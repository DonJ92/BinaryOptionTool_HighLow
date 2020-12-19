<?php
use Litipk\BigNumbers\Decimal;
/**
 * UCX Admin Page : Global common functions
 * 2019/05/09 Created by H(S
 * 
 * @author H(S
 */

/**
 * レコード配列を指定のフィールドをキーにとして
 * 再整理する。
 * 
 * array(array('id'=>1, 'name'=>'abc'), array('id'=>3, 'name'=>'xyz')) => 
 * => array(1 => array('id'=>1, 'name'=>'abc'), 3 => array('id'=>3, 'name'=>'xyz'))
 * 
 * @param array $data
 * @param string $keyField
 * @return array
 */
function g_makeArrayIDKey($data, $idField='id')
{
    $result = array();
    
    foreach ($data as $record) {
        if (!isset($record->$idField)) {
            continue;
        }
        $result[$record->$idField] = $record;
    }
    
    return $result;
}

/**
 * Get text from value of specified enumeration 
 * 
 * @param string $enumID
 * @param integer | string | null $enumValue
 * @return mixed | array
 */
function g_enum($enumID, $value = null)
{
    global $g_masterData;

    $enumArray = array();
    if (isset($g_masterData[$enumID]))
        $enumArray = $g_masterData[$enumID];
    
    if (!is_array($enumArray))
        $enumArray = array();

    // get result
    $result = array();
    if (null !== $value) {
        if (strpos($value, ',') !== false) {
            $values = explode(',', $value);
        } else {
            $values = array($value);
        }
        foreach ($values as $value) {
            if (isset($enumArray[$value])) {
                $result[] = $enumArray[$value];
            }
        }
        $result = implode(', ', $result);
        return $result;
    }

    return $enumArray;
}

/**
 * Get specified element from object safely
 *
 * @param array $arr
 * @param string $key1
 * @param string $key2
 * @param mixed $default
 * @return mixed
 */
function g_getArrayValue($arr, $key1, $key2 = '', $default = '')
{
    if (empty($key2)) {
        return (isset($arr[$key1]) ? $arr[$key1] : $default);
    }

    return (isset($arr[$key1][$key2]) ? $arr[$key1][$key2] : $default);
}

/**
 * Get specified element from object safely
 * (参照：g_getArrayValue とほぼ同じ)
 *
 * @param array $array
 * @param string $key
 * @param mixed $default
 * @return mixed
 */
function g_getValue($array, $key, $default)
{
    $key = (string) $key;
    if (isset($array[$key])) {
        return $array[$key];
    }
    return $default;
}

/**
 * Format number safely
 * 32432.232 => "32,432.23"
 * 0 => ""
 * @param number $val
 * @param number $precision=2
 * @return string
 * @author KIS
 */
function g_safeNumberFormat($val, $precision=0)
{
    if (empty($val) || $val == 0) {
        return '';
    }
    
    return number_format($val, $precision, '.', ' ');
}

/**
 * Format number from text
 * 32432.232 => "32,432.23"
 * 0 => "0.00"
 * @param number $val
 * @param number $precision=2
 * @return string
 * @author KIS
 */
function g_numberFormat($val, $precision=0)
{
    return number_format($val, $precision, '.', ' ');
}

/**
 * Return integer value safely
 * 0 => ""
 * 
 * @author KHU
 * @param number $val
 * @return string
 */
function g_safeInteger($val, $precision=0)
{
    if (empty($val) || $val == 0) {
        return '';
    }

    return $val;
}

/**
 * Get date after specified days
 * e.g., 2016/04/16, 5 => 2016/04/21
 *
 * @author KHU
 * @param string $date
 * @param int $pastDays
 * @return string
 */
function g_dateAfterNDay($date, $pastDays) {
    $ts = strtotime($date);
    $ts += $pastDays * 86400/*1day*/;
    return date('Y-m-d', $ts);
}

/**
 * Calculate days between two date
 * ex) 2016/03/02, 2016/05/05 => 34
 * 
 * @param string $date1
 * @param string $date2
 * @return integer
 */
function g_dateDiff($date1, $date2 = null) {
    $ts1 = strtotime($date1);
    $ts2 = empty($date2) ? time() : strtotime($date2);
    $ret = ($ts2 - $ts1) / 86400/*1day*/;
    
    return floor($ret);
}

/**
 * Output html tag to show sort icon
 *
 * @param string $curSort
 * @param string $thisSort
 * @param string $sortOrder
 */
function g_SortIcon($curSort, $thisSort, $sortOrder) {
    if ($curSort == $thisSort) {
        echo( $sortOrder == 'DESC' ? '<i class="icon-sort-down"></i>' :
                                     '<i class="icon-sort-up"></i>');
        return;
    }

    echo '<i class="icon-sort"></i>';
}

/**
 * Output html tag to show sort icon
 * 
 * @param string $curSort
 * @param string $thisSort
 * @param string $sortOrder
 */
function g_AdminSortIcon($curSort, $thisSort, $sortOrder) {
    if (strtolower($curSort) == strtolower($thisSort)) {
        echo( strtoupper($sortOrder) == 'DESC' ? '<i class="icon-sort-down"></i>' :
                                      '<i class="icon-sort-up"></i>');
        return;
    }
    
    echo '<i class="icon-sort"></i>';
}

/**
 * MACRO: Output 'selected' option
 *
 * @param mixed $val1
 * @param string $val2
 * @param boolean $multiple
 */
function g_Selected($val1, $val2, $multiple = false) {
    if ($multiple) {
        if (!empty($val1) && in_array($val2, $val1)) {
            echo 'selected="selected"';
        }
    } else {
        if ($val1 == $val2) {
            echo 'selected="selected"';
        }
    }
}

/**
 * MACRO: Output 'checked' option
 *
 * @param mixed $val1
 * @param string $val2
 * @param boolean $multiple
 */
function g_Checked($val1, $val2, $multiple = false) {
    if ($multiple) {
        if (in_array($val2, $val1)) {
            echo 'checked="checked"';
        }
    } else {
        if ($val1 == $val2) {
            echo 'checked="checked"';
        }
    }
}

/**
 * MACRO: Output formatted error message
 * 
 * @param string $field
 * @param Illuminate\Support\ViewErrorBag $errors
 * @return void
 */
function g_renderError($field, $errors) {
    $errorBag = $errors->getBag('default');
    if (!$errorBag->has($field)) {
        return;
    }
    
    $errorMessages = $errorBag->get($field);
    $cnt = count($errorMessages);
    if ($cnt == 1) {
        echo '<span class="help-block">' . $errorMessages[0] . '</span>';
    } else {
        foreach ($errorMessages as $errorMessage) {
            echo '<span class="help-block">・' . $errorMessage . '</span>';
        }
    }
}

/**
 * Socket communication : Send and receive
 * 2016/11/17 Created by KHU
 * 2017/06/27 Updated by CJS : Add exception handler
 * 
 * @param string  $serverIP
 * @param integer $port
 * @param string  $send
 * @return string | int response
 */
function g_socketSendReceive($serverIP, $port, $send) {
//    $socketServer = config('app.socket');
    if (empty($serverIP)) {
//        $serverIP = $socketServer['host'];
        $serverIP = sys_config('ENC_IP');
    }
    if (empty($port)) {
//        $port = $socketServer['port'];
        $port = sys_config('ENC_PORT');
    }

    // Connect to server
    $socket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP/*'tcp'*/);
    if (!is_resource($socket)) {
        //echo 'Unable to create socket: '. socket_strerror(socket_last_error()) . PHP_EOL;
        return -3; // failed to create socket
    }
    //socket_set_timeout($socket, 5/*seconds*/);
    try {
        if (false === socket_connect($socket, $serverIP, $port)) {
            return -4; // failed to connect
        }
    } catch (Exception $e) {
        return -4;
    }

    // send
    socket_write($socket, $send, strlen($send)); // socket_send: MSG_EOF
    
    // receive
    $response = '';
    if (false !== ($bytes = socket_recv($socket, $response, 9999, MSG_WAITALL))) {
        //echo "Read $bytes bytes from socket_recv(). Closing socket...";
    } else {
        //echo "socket_recv() failed; reason: " . socket_strerror(socket_last_error($socket)) . "\n";
        socket_close($socket);
        return -5; // failed to receive
    }
    socket_close($socket);//echo $response; die;
    
    return substr($response, 0, $bytes);
}

/**
 * Convert date format. (method 1)
 * 20160419135347 => 2016-04-19 13:53:47
 * 2016/11/27 Created by KHU
 *
 * @author KHU
 * @param string $date
 * @return string
 */
function g_convString2Date($date) {
    // 20160419135347 => 2016-04-19 13:53:47
    if (strlen($date) > 14 || count(explode('-', $date)) > 1) {
        return $date;
    }
    $result = 
        substr($date,  0,  4) . '-' . // year
        substr($date,  4,  2) . '-' . // month
        substr($date,  6,  2) . ' ' . // day
        substr($date,  8,  2) . ':' . // hour
        substr($date, 10,  2) . ':' . // minute
        substr($date, 12,  2);        // second
    
    return $result;
}

/**
 * Convert date format. (method 2)
 * 2016-04-19 13:53:47 => 20160419135347
 * 2016/11/27 Created by KHU
 *
 * @author KHU
 * @param string $date
 * @return string
 */
function g_convDate2String($date) {
    // 2016-04-19 13:53:47 => 20160419135347
    if (count(explode('-', $date)) < 3) {
        return $date;
    }
    $result = date('YmdHis', strtotime($date));
    return $result;
}

/**
 * Compare two dates
 * Format1: 20160419135347
 * Format2: 2016-04-19 13:53:47
 * 2016/11/27 KHU
 * 
 * @author KHU
 * @param string $date1
 * @param string $date2
 * @return int (-1: <, 0: =, 1: >)
 */
function g_compareDate($date1, $date2) {
    $date1 = g_convDate2String($date1);
    $date2 = g_convDate2String($date2);
    
    if ($date1 == $date2) {
        return 0;
    }
    return $date1 < $date2 ? -1 : 1;
}

/**
 * Execute remote command on specified server
 * 2017/05/06 KHU
 *
 * @author KHU
 * @param String $serverIP
 * @param String $command
 * @return String output
 */
function g_remoteShell($serverIP, $command) {
    //$sshCommand = 'ssh -l root ' . $serverIP . ' "cd ' . $logPath . ';cat ' . $logFile . '"';
    $sshCommand = 'ssh -l root ' . $serverIP . ' "' . $command . '"';

    ob_start();
    passthru($sshCommand);
    $result = ob_get_clean();

    return $result;
}

/**
 * Send http-request with POST method
 * 2017/07/03 RCM
 *
 * @param $url
 * @param $content
 * @return string
 */
/*
function g_sendHttpRequest($url, $content, $method='POST') {
    try {
        $postdata = http_build_query($content);
        $opts = array('http' =>
            array(
                'method' => $method,
                'header' => array(
                    'User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36',
                    'Content-type: application/x-www-form-urlencoded',
                ),
                'content' => $postdata
            )
        );
        $context = stream_context_create($opts);
        $stream = fopen($url, 'r', false, $context);
        $ret = stream_get_contents($stream);
        fclose($stream);
        return $ret;
    } catch (Exception $e) {
        //echo $e->getMessage();
        return '';
    }
}
*/
function g_sendHttpRequest($url, $method=HTTP_METHOD_GET, $params = [], $headers = "") {
    $ch = curl_init();  

    curl_setopt($ch, CURLOPT_URL, $url);
    if (!empty($headers)) {
        curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
    }

    curl_setopt($ch, CURLOPT_RETURNTRANSFER,true);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    if ($method == HTTP_METHOD_POST) {
        curl_setopt($ch, CURLOPT_POST, true);
        curl_setopt($ch, CURLOPT_POSTFIELDS, $params);
    }

    $res = curl_exec($ch);

    return $res;
}


/**
 * Get system configuration value specified by name from db
 *
 * @param  string option_name
 * @return array()
 */
function sys_config($optionName) {
//    $systemConfigData = DB::table('T_SYSTEM_CONFIG')->lists('option_value', 'option_name');
    $systemConfigData = DB::table('T_SYSTEM_CONFIG')
        ->where('option_name', $optionName)
        ->select('option_value')
        ->get();

    return $systemConfigData[0]->option_value;
}

/**
 * Get value of specified option for current staff
 *
 * @param  string $itemName
 * @return integer
 */
function g_getProfile($itemName, $default=null) {
    global $g_profile;

    if (empty($g_profile[$itemName])) {
        return $default;
    } else {
        return $g_profile[$itemName];
    }
}

/**
 * Format large number with unit such as k, M 
 * (ex: 15000 -> 15k)
 *
 * @param integer $number
 * @return string
 */
function g_numberFormatWithUnit($number) {
    if ($number < 10000) {
        $temp = explode('.', $number);
        if (!empty($temp[1])) {
            if (count($temp[1]) >= 2 && substr($temp[1],1,1) >= 5) {
                return number_format($number + 0.1, 1, '.', ',');
            } else {
                return number_format($number, 1, '.', ',');
            }
        } else {
            return number_format($number, 0, '.', ',');
        }
    } elseif ($number > 10000 && $number < 1000000) {
        $number = $number/10000;
        $temp = explode('.', $number);
        if (!empty($temp[1])) {
            if (count($temp[1]) >= 2 && substr($temp[1],1,1) >= 5) {
                return number_format($number + 0.1, 1, '.', ',') . '만';
            } else {
                return number_format($number, 1, '.', ',') . '만';
            }
        } else {
            return number_format($number, 0, '.', ',') . '만';
        }
    } elseif ($number > 1000000 && $number < 100000000) {
        $number = $number/10000;
        $temp = explode('.', $number);
        if (!empty($temp[1])) {
            if (count($temp[1]) >= 2 && substr($temp[1],1,1) >= 5) {
                return number_format($number + 0.1, 1, '.', ',') . '만';
            } else {
                return number_format($number, 1, '.', ',') . '만';
            }
        } else {
            return number_format($number, 0, '.', ',') . '만';
        }
    } elseif ($number > 100000000 && $number < 10000000000) {
        $number = $number/100000000;
        $temp = explode('.', $number);
        if (!empty($temp[1])) {
            if (count($temp[1]) >= 2 && substr($temp[1],1,1) >= 5) {
                return number_format($number + 0.1, 1, '.', ',') . '억';
            } else {
                return number_format($number, 1, '.', ',') . '억';
            }
        } else {
            return number_format($number, 0, '.', ',') . '억';
        }
    } else {
        $number = $number/100000000;
        $temp = explode('.', $number);
        if (!empty($temp[1])) {
            if (count($temp[1]) >= 2 && substr($temp[1],1,1) >= 5) {
                return number_format($number + 0.1, 1, '.', ',') . '억';
            } else {
                return number_format($number, 1, '.', ',') . '억';
            }
        } else {
            return number_format($number, 0, '.', ',') . '억';
        }
    }
}

/**
 * Write staff's operation history
 * 2018/04/18 Created by KHU
 *
 * @param $adminID
 * @param $adminName
 * @param $ipAddr
 * @param $opType
 * @param $opNote
 * @param $opResult
 * @param string $opAt
 * @param int $moduleID
 * @param int $funcID
 * @return bool
 */
function g_writeAdminLog($adminID, $adminName, $ipAddr,
                         $opType, $opNote, $opResult, $opAt = '',
                         $moduleID = 0, $funcID = 0
) {
    $adminLogTbl = new \App\Models\ManagerHistory();
    $result = $adminLogTbl->registerNewLog(
        $adminID, $adminName, $ipAddr,
        $opType, $opNote, $opResult, $opAt,
        $moduleID, $funcID
    );
    return $result;
}

/**
 * Get system settings
 */
function g_getSystemSettings() {
    $systemSettings = Cache::get('system-settings', null);
    if (empty($systemSettings)) {
        //throw new \Exception('Could not load system-settings from cache');
    }

    return $systemSettings;
}

/**
 * Get business date of today
 */
function g_getToday() {
    $systemSettings = g_getSystemSettings();
    if (empty($systemSettings)) {
        return false;
    }

    return $systemSettings['TODAY'];
}

/**
 * Sum arrays maintaining their structure
 *
 * @param $records
 * @return array|mixed
 */
function g_sumAssocArray($records) {
    $sum = array();
    foreach ($records as $record) {
        $sum = g_addAssocArray($sum, $record);
    }

    return $sum;
}

/**
 * Add two array maintaining their structure
 *
 * @param $record1
 * @param $record2
 * @return mixed
 */
function g_addAssocArray($record1, $record2) {
    foreach ($record2 as $field => $value) {
        if (is_object($value)) {
            $value = (array)$value;
        }
        if (!isset($record1[$field])) {
            $record1[$field] = $value;
            continue;
        }

        if (is_array($value)) {
            $record1[$field] = g_addAssocArray((array)$record1[$field], $value);
        } else if (is_numeric($value)) {
            $record1[$field] += $value;
        }
    }

    return $record1;
}

/**
 * Calculate increasing rate of two numeric value
 * 100, 105, 0  => + 5%
 * 100,  95, 0  => - 5%
 * 200, 115, 1  => - 42.5%
 *
 * @param $before
 * @param $after
 * @param $precision
 * @return string
 */
function g_calcIncRate($before, $after, $precision = 0) {
    if ($before == $after) {
        return '';
    }

    $result = $after > $before ? '+ ' : '- ';
    if ($before != 0) {
        $result .= round(abs(($after - $before) * 100 / $before), $precision);
    } else {
        $result .= '100';
    }

    $result .= '%';

    return $result;
}

function cAsset($path)
{
    if (env('APP_ENV') === 'production') {
        return secure_url(ltrim($path, '/'));
    }

    return url(ltrim($path, '/'));
}

function cUrl($path) {
    if (env('APP_ENV') === 'production') {
        return secure_url($path);
    }

    return url($path);
}

function br2nl($string) {
    return preg_replace('/\<br\>(\s*)?\/?/i', "\n", $string);
}

function my_encrypt($data, $key)
{
    $l = strlen($key);
    if ($l < 16)
        $key = str_repeat($key, ceil(16/$l));

    if ($m = strlen($data)%8)
        $data .= str_repeat("\x00",  8 - $m);
    $val = openssl_encrypt($data, 'BF-ECB', $key, OPENSSL_RAW_DATA | OPENSSL_NO_PADDING);

    return $val;
}

function my_decrypt($data, $key)
{
    $l = strlen($key);
    if ($l < 16)
        $key = str_repeat($key, ceil(16/$l));

    $val = openssl_decrypt($data, 'BF-ECB', $key, OPENSSL_RAW_DATA | OPENSSL_NO_PADDING);

    return $val;
}

function base64_encode_url($string) {
    return str_replace(['+','/','='], ['-','_',''], base64_encode($string));
}

function base64_decode_url($string) {
    return base64_decode(str_replace(['-','_'], ['+','/'], $string));
}
