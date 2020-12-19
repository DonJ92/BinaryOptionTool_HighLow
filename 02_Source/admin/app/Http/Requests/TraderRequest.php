<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class TraderRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        $id = $this->request->get('id');

        if ($id > 0) {
            // For update
            return [
                'login_id'      => 'required|max:255|unique:mt_users,login_id,' . $id,
                'password'      => 'max:255',
                'name'          => 'required|max:255',
                'email'         => 'required|max:255',
                'status'        => 'required',
            ];
        }

        return [
            'login_id'      => 'required|max:255|unique:mt_users',
            'password'      => 'required|max:255',
            'name'          => 'required|max:255',
            'email'         => 'required|max:255',
            'status'        => 'required',
        ];
    }

    public function messages()
    {
        return [
            'login_id.required'     => trans('auth.required'),
            'login_id.max'          => trans('auth.max255'),
            'password.required'     => trans('auth.required'),
            'password.max'          => trans('auth.max255'),
            'login_id.unique'       => trans('auth.unique'),
            'name.required'         => trans('auth.required'),
            'name.max'              => trans('auth.max255'),
            'email.required'        => trans('auth.required'),
            'email.max'             => trans('auth.max255'),
            'status.required'       => trans('auth.required'),
        ];
    }
}
