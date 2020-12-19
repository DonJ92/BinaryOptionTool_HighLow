<?php

namespace App\Http\Controllers;

use Auth;
use App\Models\Signals;
use App\Models\Copies;
use Illuminate\Http\Request;

class SignalsController extends Controller
{
    public function index() {
        return view('signals.list');
    }

    public function detail(Request $request) {
        $id = $request->get('id');

        return view('signals.detail', [
            'signal_id'     => $id,
        ]);
    }

    public function ajax_search(Request $request) {
        $params = $request->all();

        $tbl = new Signals();
        $ret = $tbl->getForDatatable($params);

        return response()->json($ret);
    }

    public function ajax_detail(Request $request) {
        $params = $request->all();

        $tbl = new Copies();
        $ret = $tbl->getForDatatable($params);

        return response()->json($ret);
    }
}
