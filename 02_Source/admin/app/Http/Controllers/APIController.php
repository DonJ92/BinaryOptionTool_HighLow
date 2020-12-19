<?php

namespace App\Http\Controllers;

use Auth;
use App\Models\Traders;
use App\Models\Signals;
use App\Models\Copies;
use Illuminate\Http\Request;

class APIController extends Controller
{
    public function api_login(Request $request) {
        $params = $request->all();

        $tbl = new Traders();
        $ret = $tbl->apiLogin($params);

        return response()->json($ret);
    }

    public function api_logout(Request $request) {
        $params = $request->all();

        $tbl = new Traders();
        $ret = $tbl->apiLogout($params);

        return response()->json($ret);
    }

    public function api_sendSignal(Request $request) {
        $params = $request->all();

        $tbl = new Signals();
        $ret = $tbl->apiInsertRecord($params);

        return response()->json($ret);
    }

    public function api_getTradeHistory(Request $request) {
        $params = $request->all();

        $tbl = new Copies();
        $ret = $tbl->apiGetTradeHistory($params);

        return response()->json($ret);
    }
}
