@extends('layouts.afterlogin')

@section('title', sprintf(trans('staff.profile'), $staff->login_id))

@section('styles')
	<style>
	</style>
@endsection

@section('scripts')
	<script src="{{ cAsset('/js/staff-edit.js') }}"></script>
	<script src="{{ cAsset('/vendor/moment/moment.js') }}"></script>
	<script src="{{ cAsset('https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js') }}"></script>
@endsection

@section('contents')
    @if ($message = Session::get('flash_message'))
        <div class="alert alert-success alert-dismissible fade show">
            <button type="button" class="close" data-dismiss="alert">×</button>
            {{ trans($message) }}
        </div>
    @endif

	<div class="row">
		<div class="col-lg-12">
			<form method="post" action="{{ route('staff.post.edit') }}" enctype="multipart/form-data">
				@csrf
				@if ($errors->any())
					<div class="card-body">
						<div class="alert alert-danger">
							<ul class="m-0">
								@foreach ($errors->all() as $error)
									<li>{{ $error }}</li>
								@endforeach
							</ul>
						</div>
					</div>
				@endif
				<div class="card">
					<div class="card-body pb-2">
						<input type="hidden" name="staff_id" value="{{ $staff->id }}">
						<div class="form-group row">
							<label class="col-form-label col-sm-2 text-sm-right"><span class="text-danger">*</span> {{ trans('staff.table.login_id') }}</label>
							<div class="col-sm-10">
								<input type="text" class="form-control" name="login_id" value="{{ old('login_id', $staff->login_id) }}">
							</div>
						</div>
						<div class="form-group row">
							<label class="col-form-label col-sm-2 text-sm-right"><span class="text-danger">*</span> {{ trans('staff.table.name') }}</label>
							<div class="col-sm-10">
								<input type="text" class="form-control" name="name" value="{{ old('name', $staff->name) }}">
							</div>
						</div>
						<div class="form-group row">
							<label class="col-form-label col-sm-2 text-sm-right"><span class="text-danger">*</span> {{ trans('staff.table.password') }}</label>
							<div class="col-sm-10">
								<input type="password" class="form-control" name="password" value="{{ old('pass') }}">
							</div>
						</div>
						<div class="form-group row">
							<label class="col-form-label col-sm-2 text-sm-right">{{ trans('staff.table.pass_conf') }}</label>
							<div class="col-sm-10">
								<input type="password" class="form-control" name="password_confirmation">
							</div>
						</div>
                        <div class="form-group row">
                            <label class="col-form-label col-sm-2 text-sm-right"><span class="text-danger">*</span> {{ trans('staff.table.avatar') }}</label>
                            <div class="col-sm-10">
                                <img id="avatar-img" src="{{ cAsset('/') }}/uploads/{{ ($user->avatar == '') ? '_none.png' : (old('avatar', $staff->avatar)) }}" alt="" class="d-block ui-w-80">
                                <div class="media-body ml-3">
                                    <label class="btn btn-outline-primary btn-sm">
                                        {{ trans('ui.button.change') }}
                                        <input type="file" id="avatar" name="avatar" class="user-edit-fileinput">
                                    </label>&nbsp;
                                    <button type="button" class="btn btn-default btn-sm md-btn-flat">{{ trans('ui.button.reset') }}</button>
                                </div>
                            </div>
                        </div>
					</div>
					<hr class="border-light m-0">
				</div>

				<div class="text-center mt-3">
					<button type="submit" class="btn btn-primary"><span class="fa fa-save"></span>&nbsp;{{ trans('ui.button.update') }}</button>&nbsp;
				</div>
			</form>
		</div>
	</div>
@endsection
