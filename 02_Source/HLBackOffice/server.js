
var express = require('express');
var app = express();
var http = require('http').Server(app);
var moment = require('moment');
var mysql = require('mysql');
var JSON = require('JSON');
var io = require('socket.io')(http, {path: '/1.0.0'});      // Socket IO URL
var times_modules = require('./time');

// Config
const port = 8484;
const mysqlSettings = {
    host:                           'localhost',
    user:                           'root',
    password:                       'qorencjdcns',
    port:                           3306,
    database:                       'hlas_manage_db',
    canRetry:                       true,
    connectionLimit:                100,                         // connection count in the pool
    connectTimeout:                 60 * 60 * 1000,
    acquireTimeout:                 60 * 60 * 1000,
    supportBigNumbers: true,
    bigNumberStrings: true,
    waitForConnections:             true,                      // true: wait for release of client, false: returns error - Now set false for test
    multipleStatements:             true
};
var pool = mysql.createPool(mysqlSettings);
pool.on('connection', function(connection) {
    //console.log(times_modules.GetNowTimeStrForLog(), 'Connection established');

    // Below never get called
    connection.on('error', function(err) {
        console.log(times_modules.GetNowTimeStrForLog(), 'MySQL error', err.code);
    });

    connection.on('close', function(err) {
        console.log(times_modules.GetNowTimeStrForLog(), 'MySQL close', err);
    });
});


// Variables
var usersCount = 0;

function getMySQLConnection() {
    return new Promise(function(resolve, reject) {
        pool.getConnection(function(err, connection) {
            if (err) {
                reject(err);
            }
            else {
                resolve(connection);
            }
        });
    });
};

function executeSQL(conn, sql) {
    return new Promise(function(resolve, reject) {
        if (conn == null) {
            console.log(times_modules.GetNowTimeStrForLog(), 'ExecuteSQL has failed. No MySQL connection!');
            reject(null);
            return;
        }

        conn.query(sql, function(err, data) {
            if (err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'ExecuteSQL has failed with error. Err :', err);
                reject(err);
            }
            else {
                resolve(data);
            }
        });
    });
}



// APIs
function getTradeInfo(loginId) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "SELECT * FROM `mt_users` WHERE `login_id` = '" + loginId + "'";

            await executeSQL(conn, sql).then(function(result) {
                resolve(result);
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Getting trade info has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function getTradeHistory(traderId) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "SELECT `mt_copies`.*, `mt_signals`.`symbol`, `mt_signals`.`cmd` FROM `mt_copies` LEFT JOIN `mt_signals` ON `mt_signals`.`id` = `mt_copies`.`signal_id` " + 
                    "WHERE `mt_copies`.`trader_id` = '" + traderId + "'";

            await executeSQL(conn, sql).then(function(result) {
                resolve(result);
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Getting trade history has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function pickSignal(nowTime) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "SELECT * FROM `mt_signals` WHERE `status` = 0 AND created_at >= '" + nowTime + "'";

            await executeSQL(conn, sql).then(function(result) {
                resolve(result);
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Pick signal has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function markSignal(signalId) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "UPDATE `mt_signals` SET `status` = 1 WHERE `id` = " + signalId;

            await executeSQL(conn, sql).then(function(result) {
                resolve(1);
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Pick signal has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function addTradeHistory(params) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            // 1. Update copied count
            var sql = "UPDATE `mt_signals` SET `copied` = `copied` + 1 WHERE `id` = " + params['signal_id'];

            await executeSQL(conn, sql).then(function(result) {
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Pick signal has failed with error. Err :', err);
                reject(err);
            });

            // 2. Insert history
            sql = "INSERT INTO `mt_copies`(`signal_id`, `trader_id`, `demo_real`, `trans_mode`, `trans_timeunit`, `trans_suffix`, `order_price`, `judge_price`, " + 
                    "`order_amount`, `payout_amount`, `status`, `ordered_at`, `judged_at`) VALUES(" + 
                    params['signal_id'] + ", " + 
                    params['trader_id'] + ", " + 
                    params['demo_real'] + ", " + 
                    params['trans_mode'] + ", " + 
                    params['trans_timeunit'] + ", " + 
                    "'" + params['trans_suffix'] + "', " + 
                    params['order_price'] + ", " + 
                    params['judge_price'] + ", " + 
                    params['order_amount'] + ", " + 
                    params['payout_amount'] + ", " + 
                    params['status'] + ", " + 
                    "'" + params['ordered_at'] + "', " + 
                    "'" + params['judged_at'] + "')";

            await executeSQL(conn, sql).then(function(result) {
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Pick signal has failed with error. Err :', err);
                reject(err);
            });

            // 3. Get new inserted id
            sql = "SELECT `id` FROM `mt_copies` WHERE `trader_id` = " + params['trader_id'] + " ORDER BY `id` DESC LIMIT 1";
            await executeSQL(conn, sql).then(function(result) {
                if (result.length > 0) {
                    resolve({
                        'signal_id': params['signal_id'],
                        'history_id': result[0]['id'],
                    });
                }
                else {
                    console.log(times_modules.GetNowTimeStrForLog(), 'Get newId for copies has failed!, ID = 0');
                    resolve(0);
                }
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Add trade history has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function updateTradeHistory(params) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "UPDATE `mt_copies` SET " + 
                    "`judge_price` = " + params['judge_price'] + ", " + 
                    "`payout_amount` = " + params['payout_amount'] + ", " + 
                    "`status` = " + params['status'] + 
                    " WHERE `id` = " + params['id'];

            await executeSQL(conn, sql).then(function(result) {
                resolve(1);
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Update trade history has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

function checkAccount(userId) {
    return new Promise(async function(resolve, reject){
        await getMySQLConnection().then(async function(conn) {
            var sql = "SELECT `status` FROM `mt_users` WHERE `id` = " + userId;

            await executeSQL(conn, sql).then(function(result) {
                if (result == null || result == undefined || result.length == 0) {
                    resolve(0);
                }
                else {
                    resolve(result[0]['status']);
                }
            }, function(err) {
                console.log(times_modules.GetNowTimeStrForLog(), 'Check account has failed with error. Err :', err);
                reject(err);
            });

            conn.release();
        }, function(err) {
            console.log(times_modules.GetNowTimeStrForLog(), 'MySQL connection has failed with error. Err :', err);
            reject(err);
        });
    });
}

async function monitorSignals() {
	var nowTime = moment().format('YYYY-MM-DD HH:mm:ss');
	await pickSignal(nowTime).then(async function(result) {
		if (result.length > 0) {
			// New signal occurred
			await markSignal(result[0]['id']);
			if (usersCount > 0) {
				if (result[0]['source'] == 'RDcomboVLDMI2')
				{
					result[0]['source'] = 'YXY';
				}
				io.emit('Request:New:Order', JSON.stringify(result[0]));
			}
		}
	});
}

// Socket.io Defines
io.on('connection', function(socket){
    var user_id = ++usersCount;
    var user_name = "user" + usersCount;

    socket.user = {
        user_id: user_id,
        user_name: user_name,
    };

    console.log(times_modules.GetNowTimeStrForLog(), 'New user connected:', socket.id);

    socket.emit('Response:Login', JSON.stringify(socket.user));

    // Disconnect
    socket.on('disconnect', function(reason){
        console.log(times_modules.GetNowTimeStrForLog(), 'User disconnected: ', socket.id, 'Reason :', reason);
    });

    socket.on('Request:Get:Trade:Info', async function(loginId) {
        await getTradeInfo(loginId).then(function(result) {
            if (result == null || result.length == 0) {
                console.log('GetTradeInfo has failed!');
            }
            else {
                var traderId = result[0]['id'];
                socket.traderId = traderId;
                socket.emit('Response:Get:Trade:Info', traderId);
            }
        });
    });

    socket.on('Request:Get:Trade:History', async function(traderId) {
        await getTradeHistory(traderId).then(function(result) {
            socket.emit('Response:Get:Trade:History', JSON.stringify(result));
        });
    });

    socket.on('Request:Add:Trade:History', async function(params) {
        var data = JSON.parse(params);
        await addTradeHistory(data).then(function(result) {
            socket.emit('Response:Add:Trade:History', result);
        });
    });

    socket.on('Request:Update:Trade:History', async function(params) {
        var data = JSON.parse(params);
        await updateTradeHistory(data).then(function(result) {
            socket.emit('Response:Update:Trade:History', 1);
        });
    });

    socket.on('Request:Check:Account', async function(userId) {
        await checkAccount(userId).then(function(result) {
            socket.emit('Response:Check:Account', result);
        });
    });

    // ----------------------------- *****
});

// 4. Start Http Server
http.listen(port, function(){
    console.log(times_modules.GetNowTimeStrForLog(), 'Server has started on port ' + port + '!');
    console.log(times_modules.GetNowTimeStrForLog(), 'Waiting for connection...');
});

setInterval(monitorSignals, 100);

process.on('uncaughtException', function (err) {
  console.error(err);
  console.log(times_modules.GetNowTimeStrForLog(), "UncaughtException has occurred. TransServer NOT Exiting...");
});
