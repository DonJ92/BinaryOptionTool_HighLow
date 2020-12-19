<?php

namespace App\Http\Controllers;

use Auth;
use App\User;
use App\Models\Traders;
use Illuminate\Http\Request;
use App\Http\Requests\TraderRequest;

class UsersController extends Controller
{
    public function index() {
        return view('users.list');
    }

    public function ajax_search(Request $request) {
        $params = $request->all();

        $tbl = new Traders();
        $ret = $tbl->getForDatatable($params);

        return response()->json($ret);
    }

    public function ajax_getInfo(Request $request) {
        $id = $request->get('id');

        $tbl = new Traders();
        $ret = $tbl->getRecordById($id);

        return response()->json($ret);
    }

    public function ajax_add(TraderRequest $request) {
        $params = $request->all();

        $tbl = new Traders();
        unset($params['id']);
        $params['password'] = bcrypt($params['password']);
        $ret = $tbl->insertRecord($params);

        return response()->json($ret);
    }

    public function ajax_edit(TraderRequest $request) {
        $params = $request->all();

        $id = $params['id'];
        $tbl = new Traders();
        unset($params['id']);
        if (isset($params['password'])) {
            if ($params['password'] == '') {
                unset($params['password']);
            }
            else {
                $params['password'] = bcrypt($params['password']);
            }
        }
		else {
			unset($params['password']);
		}
        $ret = $tbl->updateRecord($id, $params);

        return response()->json($ret);
    }

    public function ajax_delete(Request $request) {
        $id = $request->get('id');

        $tbl = new Traders();
        $ret = $tbl->deleteRecordById($id);

        return response()->json($ret);
    }

    public function ajax_activateAll(Request $request) {
        $ids = $request->get('ids');

        $tbl = new Traders();
        $ret = $tbl->updateAllStatus($ids,STATUS_ACTIVE);

        return response()->json($ret);
    }

    public function ajax_bannAll(Request $request) {
        $ids = $request->get('ids');

        $tbl = new Traders();
        $ret = $tbl->updateAllStatus($ids, STATUS_BANNED);

        return response()->json($ret);
    }
}
