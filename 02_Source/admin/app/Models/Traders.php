<?php

namespace App\Models;

use DB;
use Log;
use Illuminate\Database\Eloquent\Model;

class Traders extends Model
{
    protected $table = 'mt_users';

    public function apiLogin($params) {
        $response = array(
            'result'        => API_RESULT_FAILURE,
            'error'         => API_ERROR_RUNTIME,
        );

        try {
            $records = DB::table($this->table)
                ->where('login_id', $params['login_id'])
                ->select('*')
                ->get();

            if (!isset($records) || count($records) == 0) {
                $response['error'] = API_ERROR_INVALID_USER;
                return $response;
            }
            if ($records[0]->loginned == USER_LOGINNED) {
                $response['error'] = API_ERROR_ALREADY_LOGINNED;
                return $response;
            }
            if ($records[0]->status == STATUS_BANNED) {
                $response['error'] = API_ERROR_BANNED_USER;
                return $response;
            }
            if (!password_verify($params['password'], $records[0]->password)) {
                $response['error'] = API_ERROR_INVALID_PASSWORD;
                return $response;
            }

            // Success
            /*$ret = DB::table($this->table)
                ->where('id', $records[0]->id)
                ->update([
                    'loginned'      => USER_LOGINNED,
                ]);*/
            $response['result'] = API_RESULT_SUCCESS;
            $response['detail'] = API_ERROR_NONE;

            return $response;
        }
        catch (\Exception $ex) {
            Log::error($ex->getMessage());
            return $response;
        }
    }

    public function apiLogout($params) {
        $response = array(
            'result'        => API_RESULT_FAILURE,
            'error'         => API_ERROR_RUNTIME,
        );

        try {
            $records = DB::table($this->table)
                ->where('login_id', $params['login_id'])
                ->select('id')
                ->get();

            if (!isset($records) || count($records) == 0) {
                $response['error'] = API_ERROR_INVALID_USER;
                return $response;
            }
            if ($records[0]->status == STATUS_BANNED) {
                $response['error'] = API_ERROR_BANNED_USER;
                return $response;
            }

            // Success
            $ret = DB::table($this->table)
                ->where('id', $records[0]->id)
                ->update([
                    'loginned'      => USER_NOT_LOGINNED,
                ]);
            $response['result'] = API_RESULT_SUCCESS;
            $response['detail'] = API_ERROR_NONE;

            return $response;
        }
        catch (\Exception $ex) {
            Log::error($ex->getMessage());
            return $response;
        }
    }

    public static function findUserId($login_id) {
        $records = self::where('login_id', $login_id)
            ->select('id')
            ->get();

        if (!isset($records) || count($records) == 0) {
            return 0;
        }

        return $records[0]->id;
    }

    public function getRecordById($id) {
        $records = DB::table($this->table)
            ->where('id', $id)
            ->select('*')
            ->get();

        if (!isset($records) || count($records) == 0) {
            return [];
        }

        return $records[0];
    }

    public function insertRecord($params) {
        $ret = DB::table($this->table)
            ->insert($params);

        return $ret;
    }

    public function updateRecord($id, $params) {
        $ret = DB::table($this->table)
            ->where('id', $id)
            ->update($params);

        return $ret;
    }

    public function deleteRecordById($id) {
        $ret = DB::table($this->table)
            ->where('id', $id)
            ->delete();

        return $ret;
    }

    public function updateAllStatus($ids, $status) {
        $ret = DB::table($this->table)
            ->whereIn('id', $ids)
            ->update([
                'status'    => $status,
            ]);

        return $ret;
    }

    public function getForDatatable($params) {
        $selector = DB::table($this->table)
            ->select(
                '*'
            );

        // filtering
        if (isset($params['columns'][1]['search']['value'])
            && $params['columns'][1]['search']['value'] !== ''
        ) {
            $selector->where('login_id', 'like', '%' . $params['columns'][1]['search']['value'] . '%');
        }
        if (isset($params['columns'][2]['search']['value'])
            && $params['columns'][2]['search']['value'] !== ''
        ) {
            $selector->where('name', 'like', '%' . $params['columns'][2]['search']['value'] . '%');
        }
        if (isset($params['columns'][3]['search']['value'])
            && $params['columns'][3]['search']['value'] !== ''
        ) {
            $selector->where('email', 'like', '%' . $params['columns'][3]['search']['value'] . '%');
        }
        if (isset($params['columns'][8]['search']['value'])
            && $params['columns'][8]['search']['value'] !== ''
        ) {
            $selector->where('status', $params['columns'][8]['search']['value']);
        }
        if (isset($params['columns'][9]['search']['value'])
            && $params['columns'][9]['search']['value'] !== ''
        ) {
            $amountRange = preg_replace('/[\$\,]/', '', $params['columns'][9]['search']['value']);
            $elements = explode(':', $amountRange);

            if ($elements[0] != "" || $elements[1] != "") {
                $elements[0] .= ' 00:00:00';
                $elements[1] .= ' 23:59:59';
                $selector->whereBetween('created_at', $elements);
            }
        }

        // number of filtered records
        $recordsFiltered = $selector->count();

        // sort
        foreach ($params['order'] as $order) {
            $field = $params['columns'][$order['column']]['data'];
            $selector->orderBy($field, $order['dir']);
        }

        // offset & limit
        if (!empty($params['start']) && $params['start'] > 0) {
            $selector->skip($params['start']);
        }

        if (!empty($params['length']) && $params['length'] > 0) {
            $selector->take($params['length']);
        }

        // get records
        $records = $selector->get();

        return [
            'draw' => $params['draw']+0,
            'recordsTotal' => DB::table($this->table)->count(),
            'recordsFiltered' => $recordsFiltered,
            'data' => $records,
            'error' => 0,
        ];
    }
}
