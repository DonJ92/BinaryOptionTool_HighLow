<?php

use Illuminate\Database\Seeder;

class UserSeeder extends Seeder
{
    protected $table = 'mt_staff';

    protected $admin_id = 'mt_admin';
    protected $admin_pass = 'mt_admin#2020';

    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        print_r("UserSeeder has started!\n");

        $records = DB::table($this->table)
            ->where('login_id', $this->admin_id)
            ->select('id')
            ->get();
        if (isset($records) && count($records) > 0) {
            return;
        }

        $ret = DB::table($this->table)
            ->insert([
                'login_id'      => $this->admin_id,
                'name'          => 'Administrator',
                'password'      => bcrypt($this->admin_pass),
                'avatar'        => '',
                'status'        => STATUS_ACTIVE,
            ]);

        print_r("UserSeeder has finished!\n");
    }
}
