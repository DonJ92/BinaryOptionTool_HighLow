<?php

use Illuminate\Support\Facades\Route;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', function () {
    return redirect()->route('login');
});
Route::get('/logout', function() {
    Auth::logout();
    return redirect()->route('login');
})->name('logout');

# Api
Route::post('/api/login', 'APIController@api_login');
Route::post('/api/logout', 'APIController@api_logout');
Route::post('/api/sendSignal', 'APIController@api_sendSignal');
Route::post('/api/getTradeHistory', 'APIController@api_getTradeHistory');

Auth::routes();

Route::group(['middleware' => 'auth'], function () {
    # Home
    Route::get('/home', 'HomeController@index')->name('home');

    # Staff
    Route::get('/staff', 'StaffController@index')->name('staff');
    Route::get('/staff/add', 'StaffController@add')->name('staff.add');
    Route::get('/staff/edit', 'StaffController@edit')->name('staff.edit');
    Route::post('/staff/add', 'StaffController@post_add')->name('staff.post.add');
    Route::post('/staff/edit', 'StaffController@post_edit')->name('staff.post.edit');
    Route::post('ajax/staff/search', 'StaffController@ajax_search');
    Route::post('ajax/staff/createToken', 'StaffController@ajax_createToken');
    Route::post('ajax/staff/delete', 'StaffController@ajax_delete');

    # Users
    Route::get('/users', 'UsersController@index')->name('users');
    Route::post('ajax/users/search', 'UsersController@ajax_search');
    Route::post('ajax/users/getInfo', 'UsersController@ajax_getInfo');
    Route::post('ajax/users/add', 'UsersController@ajax_add');
    Route::post('ajax/users/edit', 'UsersController@ajax_edit');
    Route::post('ajax/users/delete', 'UsersController@ajax_delete');
    Route::post('ajax/users/activateAll', 'UsersController@ajax_activateAll');
    Route::post('ajax/users/bannAll', 'UsersController@ajax_bannAll');

    # Signals
    Route::get('/signals', 'SignalsController@index')->name('signals');
    Route::get('/signals/detail', 'SignalsController@detail')->name('signals.detail');
    Route::post('ajax/signals/search', 'SignalsController@ajax_search');
    Route::post('ajax/signals/detail', 'SignalsController@ajax_detail');
});
