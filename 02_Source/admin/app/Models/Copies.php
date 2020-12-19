<?php

namespace App\Models;

use DB;
use Log;
use Illuminate\Database\Eloquent\Model;

class Copies extends Model
{
    protected $table = 'mt_copies';
    protected $table_signals = 'mt_signals';
    protected $table_users = 'mt_users';

    public function apiGetTradeHistory($params) {
        $response = array(
            'result'        => API_RESULT_FAILURE,
            'error'         => API_ERROR_RUNTIME,
            'detail'        => [],
        );

        try {
            $user_id = Traders::findUserId($params['login_id']);
            if ($user_id == 0) {
                // No trader
                return $response;
            }

            $records = DB::table($this->table)
                ->leftJoin($this->table_signals, $this->table_signals . '.id', '=', $this->table . '.signal_id')
                ->where($this->table . '.trader_id', $user_id)
                ->select(
                    $this->table . '.*',
                    $this->table_signals . '.symbol',
                    $this->table_signals . '.cmd'
                )
                ->get();

            if (!isset($records) || count($records) == 0) {
                $response['error'] = API_ERROR_NO_DATA;
                return $response;
            }

            $response = array(
                'result'        => API_RESULT_SUCCESS,
                'error'         => API_ERROR_NONE,
                'detail'        => $records,
            );
            return $response;
        }
        catch (\Exception $ex) {
            Log::error($ex->getMessage());
            return $response;
        }
    }

    public function getForDatatable($params) {
        $selector = DB::table($this->table)
            ->leftJoin($this->table_signals, $this->table_signals . '.id', '=', $this->table . '.signal_id')
            ->leftJoin($this->table_users, $this->table_users . '.id', '=', $this->table . '.trader_id')
            ->select(
                $this->table . '.*',
                $this->table_signals . '.source',
                $this->table_signals . '.symbol',
                $this->table_users . '.name as trader'
            );

        // filtering
        if (isset($params['columns'][3]['search']['value'])
            && $params['columns'][3]['search']['value'] !== ''
        ) {
            $selector->where($this->table_users . '.name', 'like', '%' . $params['columns'][3]['search']['value'] . '%');
        }
        if (isset($params['columns'][9]['search']['value'])
            && $params['columns'][9]['search']['value'] !== ''
        ) {
            $selector->where($this->table . '.status', $params['columns'][9]['search']['value']);
        }
        if (isset($params['columns'][10]['search']['value'])
            && $params['columns'][10]['search']['value'] !== ''
        ) {
            $amountRange = preg_replace('/[\$\,]/', '', $params['columns'][10]['search']['value']);
            $elements = explode(':', $amountRange);

            if ($elements[0] != "" || $elements[1] != "") {
                $elements[0] .= ' 00:00:00';
                $elements[1] .= ' 23:59:59';
                $selector->whereBetween('ordered_at', $elements);
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
