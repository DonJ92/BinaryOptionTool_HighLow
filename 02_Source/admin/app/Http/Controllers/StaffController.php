<?php

namespace App\Http\Controllers;

use Auth;
use App\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;

class StaffController extends Controller
{
    public function index() {
        $user = Auth::user();
        if ($user->role != USER_ROLE_ADMIN) {
            return redirect()->back();
        }

        $tbl = new User();
        $users = $tbl->getAll();

        return view('staff.list', [
            'users'     => $users,
        ]);
    }

    public function add()
    {
        $tbl = new User();
        $users = $tbl->getAll();

        $user = Auth::user();
        if ($user->role != USER_ROLE_ADMIN)
        {
            return redirect()->back();
        }

        return view('staff.add', [
            'users'     => $users,
        ]);
    }

    public function edit(Request $request)
    {
        $tbl = new User();
        $users = $tbl->getAll();

        $id = $request->get('id');
        $user = Auth::user();

        $tbl = new User();
        $staff = $tbl->getRecordById($id);

        return view('staff.edit', [
            'id'    => $id,
	        'user'  => $user,
            'users' => $users,
            'staff' => $staff,
        ]);
    }

    public function profile()
    {
        $tbl = new User();
        $users = $tbl->getAll();

        $user = Auth::user();

        $tbl = new User();
        $staff = $tbl->getRecordById($user->id);

        return view('staff.edit', [
            'id'    => $user->id,
            'staff' => $staff,
            'users' => $users,
        ]);
    }

    public function post_add(Request $request)
    {
        $user = Auth::user();
        if ($user->role != USER_ROLE_ADMIN)
        {
            return redirect()->back();
        }

        $this->validate($request, [
            'login_id'      => 'required|max:64|unique:ems_staff',
            'name'          => 'required|max:64',
            'password'      => 'required|min:6|max:255|confirmed',
            'role'          => 'required',
            'auth_token'    => 'required',
        ]);

        $record = $request->only('login_id', 'name', 'password', 'email', 'role', 'auth_token');
        $record['password'] = bcrypt($record['password']);
        $record['avatar'] = 'none.png';
        $record['status'] = STATUS_ACTIVE;

        $tbl = new User();
        $tbl->createRecord($record);

        return redirect()->route('staff')
            ->with('flash_message', 'staff.message.add_success');
    }

    public function post_edit(Request $request) {
        $user = Auth::user();
        $id = $request->get('staff_id');

        // Validate name, email and password fields
        $this->validate($request, [
            'login_id'      => 'required|max:64|unique:mt_staff,login_id,' . $id,
            'name'          => 'required|max:64',
            'password'      => 'confirmed',
        ]);

        // Retrieving only the name, login-id and password data
        $input = $request->only('login_id', 'name', 'password', 'avatar');
        if (empty($input['password'])) {
            unset($input['password']);
        } else {
            $input['password'] = bcrypt($input['password']);
        }

        $file = $request->file('avatar');
        if (isset($file)) {
            $extension = $file->getClientOriginalExtension();
            $userId = $request->get('login_id');
            $fileName = $userId . '.' . $extension;
            $ret = $file->move('uploads', $fileName);
            $input['avatar'] = $fileName;
        }

        $userTbl = new User();
        $userTbl->updateRecordById($id, $input);

        return redirect()->route('staff.edit', ['id' => $id])
            ->with('flash_message', 'staff.message.edit_success');
    }

    public function ajax_search(Request $request)
    {
        $user = Auth::user();
        if ($user->role != USER_ROLE_ADMIN)
        {
            return redirect()->back();
        }

        $params = $request->all();

        $tbl = new User();
        $ret = $tbl->getForDatatable($params);

        return response()->json($ret);
    }

    public function ajax_delete(Request $request)
    {
        $user = Auth::user();
        if ($user->role != USER_ROLE_ADMIN)
        {
            return redirect()->back();
        }

        $id = $request->get('id');

        $tbl = new User();
        $ret = $tbl->deleteRecordById($id);

        return response()->json($ret);
    }

    public function ajax_createToken(Request $request) {
        $tbl = new User();
        $token = $tbl->createToken();

        return response()->json($token);
    }
}
