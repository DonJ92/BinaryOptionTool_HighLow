<?php
/**
 * MLM Admin Page : Master data
 * 2020/05/16 Created by RedSpider
 *
 * @author RedSpider
 */

# Role Data
define('USER_ROLE_ADMIN', 1);
define('USER_ROLE_MANAGER', 2);
define('USER_ROLE_SUBSCRIBER', 3);
$UserRoleData = array(
    USER_ROLE_ADMIN         => ['管理者', 'danger'],
    USER_ROLE_MANAGER       => ['マネージャ', 'info'],
    USER_ROLE_SUBSCRIBER	=> ['ユーザー', 'primary'],

);

# User Gender
define('USER_GENDER_MALE', 0);
define('USER_GENDER_FEMALE', 1);
$UserGenderData = array(
    USER_GENDER_MALE     =>  ['男', 'primary'],
    USER_GENDER_FEMALE   =>  ['女', 'info'],
);


# Status
define('STATUS_BANNED', 0);
define('STATUS_ACTIVE', 1);
$StatusData = array(
    STATUS_BANNED       =>  ['無効', 'danger'],
    STATUS_ACTIVE       =>  ['有効', 'success'],
);

# Singal Types
define('SIGNAL_BUY', 0);
define('SIGNAL_SELL', 1);
$SignalTypeData = array(
    SIGNAL_BUY          => ['Buy', 'primary'],
    SIGNAL_SELL         => ['Sell', 'info'],
);

# Signal Status
define('SIGNAL_STATUS_NEW', 0);
define('SIGNAL_STATUS_PROCESSED', 1);
$SignalStatusData = array(
    SIGNAL_STATUS_NEW           => ['新規', 'danger'],
    SIGNAL_STATUS_PROCESSED     => ['配信完了', 'success'],
);

# Trans Modes
define('TRANS_MODE_HIGHLOW', 1);
define('TRANS_MODE_HIGHLOW_SPREAD', 2);
define('TRANS_MODE_TURBO', 3);
define('TRANS_MODE_TURBO_SPREAD', 4);
$TransModeData = array(
    TRANS_MODE_HIGHLOW          => ['HighLow', 'primary'],
    TRANS_MODE_HIGHLOW_SPREAD   => ['HighLow-Spread', 'info'],
    TRANS_MODE_TURBO            => ['Turbo', 'success'],
    TRANS_MODE_TURBO_SPREAD     => ['Turbo-Spread', 'warning'],
);

# Trans TimeUnits
define('TRANS_TIMEUNIT_30S', 30);
define('TRANS_TIMEUNIT_15M', 900);
define('TRANS_TIMEUNIT_1D', 86400);
$TransTimeUnitsData = array(
    TRANS_TIMEUNIT_30S          => ['30秒', 'primary'],
    TRANS_TIMEUNIT_15M          => ['15分', 'info'],
    TRANS_TIMEUNIT_1D           => ['24時間', 'danger'],
);

# Copies Status
define('COPIES_STATUS_NONE', 0);
define('COPIES_STATUS_PENDING', 1);
define('COPIES_STATUS_COMPLETED', 2);
define('COPIES_STATUS_SOLD', 3);
$CopiesStatusData = array(
    COPIES_STATUS_NONE          => ['なし', 'danger'],
    COPIES_STATUS_PENDING       => ['注文中', 'primary'],
    COPIES_STATUS_COMPLETED     => ['完了', 'success'],
    COPIES_STATUS_SOLD          => ['専売', 'warning'],
);

# Demo/Real
define('COPIES_DEMO', 0);
define('COPIES_REAL', 1);
$CopiesDemoRealData = array(
    COPIES_DEMO                 => ['デモー', 'primary'],
    COPIES_REAL                 => ['レアル', 'danger'],
);

$g_masterData = array(
    'UserRoleData'          => $UserRoleData,
    'UserGenderData'        => $UserGenderData,
    'StatusData'            => $StatusData,

    'SignalTypeData'        => $SignalTypeData,
    'SignalStatusData'      => $SignalStatusData,

    'TransModeData'         => $TransModeData,
    'TransTimeUnitsData'    => $TransTimeUnitsData,
    'CopiesStatusData'      => $CopiesStatusData,
    'CopiesDemoRealData'    => $CopiesDemoRealData,
);
