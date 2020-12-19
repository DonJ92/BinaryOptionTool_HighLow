@extends('layouts.afterlogin')

@section('title', trans('users.title'))

@section('styles')
    <link href="{{ cAsset('vendor/datatables/datatables.css') }}" rel="stylesheet">
    <link href="{{ cAsset('vendor/daterangepicker/daterangepicker.css') }}" rel="stylesheet">
    <link href="{{ cAsset('app-assets/css/custom.css') }}" rel="stylesheet">
@endsection

@section('contents')
    <!-- users list start -->
    <section class="users-list-wrapper">
        <!-- users filter start -->
        <div class="card">
            <div class="card-header">
                <h4 class="card-title">{{ trans('ui.search.filters') }}</h4>
                <a class="heading-elements-toggle"><i class="fa fa-ellipsis-v font-medium-3"></i></a>
            </div>
            <div class="card-content collapse show">
                <div class="card-body">
                    <div class="users-list-filter">
                        <form>
                            <div class="row">
                                <div class="col-md">
                                    <label class="form-label">{{ trans('users.table.login_id') }}</label>
                                    <input type="text" id="filter-login_id" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('users.table.name') }}</label>
                                    <input type="text" id="filter-name" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('users.table.email') }}</label>
                                    <input type="text" id="filter-email" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('users.table.status') }}</label>
                                    <select id="filter-status" class="form-control">
                                        <option value="">{{ trans('ui.search.any') }}</option>
                                        @foreach (g_enum('StatusData') as $index => $status)
                                            <option value="{{ $index }}">{{ $status[0] }}</option>
                                        @endforeach
                                    </select>
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('users.table.reged_at') }}</label>
                                    <input type="text" id="filter-date" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md col-xl-2">
                                    <label class="form-label">&nbsp;</label>
                                    <button type="button" onclick="javascript:doSearch()" class="btn btn-primary btn-block">
                                        <i class="fa fa-search"></i>&nbsp;{{ trans('ui.button.search') }}
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <!-- users filter end -->

        <input type="hidden" id="edit-caption" value="{{ trans('ui.button.edit') }}">
        <input type="hidden" id="delete-caption" value="{{ trans('ui.button.delete') }}">
        <?php
            echo '<script>';
            echo 'var UserGenderData = ' . json_encode(g_enum('UserGenderData')) . ';';
            echo 'var StatusData = ' . json_encode(g_enum('StatusData')) . ';';
            echo '</script>';
        ?>

        @if ($message = Session::get('flash_message'))
            <div class="alert alert-success alert-dismissible fade show">
                <button type="button" class="close" data-dismiss="alert">×</button>
                {{ trans($message) }}
            </div>
        @endif

        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <button type="button" id="btn-add" class="text-white btn btn-primary">
                        <i class="fa fa-plus"></i>&nbsp;{{ trans('ui.button.add') }}
                    </button>
                    <button type="button" id="btn-activate-all" class="text-white btn btn-success" disabled>
                        <i class="fa fa-check"></i>&nbsp;{{ trans('ui.button.activate') }}
                    </button>
                    <button type="button" id="btn-bann-all" class="text-white btn btn-danger" disabled>
                        <i class="fa fa-times"></i>&nbsp;{{ trans('ui.button.bann') }}
                    </button>
                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-content">
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="users-list" class="table">
                            <thead class="text-center">
                            <tr>
                                <th><input id="checkPageItemAll" name="checkPageItemAll" type="checkbox" ></th>
                                <th>{{ trans('users.table.no') }}</th>
                                <th>{{ trans('users.table.login_id') }}</th>
                                <th>{{ trans('users.table.name') }}</th>
                                <th>{{ trans('users.table.email') }}</th>
                                <th>{{ trans('users.table.gender') }}</th>
                                <th>{{ trans('users.table.birthday') }}</th>
                                <th>{{ trans('users.table.mobile') }}</th>
                                <th>{{ trans('users.table.address') }}</th>
                                <th>{{ trans('users.table.status') }}</th>
                                <th>{{ trans('users.table.reged_at') }}</th>
                                <th>{{ trans('users.table.actions') }}</th>
                            </tr>
                            </thead>
                            <tbody class="text-center">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- users list ends -->

    <!-- Modal template -->
    <div class="modal fade" id="modal-users">
        <div class="modal-dialog">
            <form id="frm-modal" class="modal-content">
                {{ csrf_field() }}
                <input type="hidden" id="edit-id">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <span id="modal-title">{{ trans('users.add_title') }}</span>
                        <br>
                        <small class="text-muted">{{ trans('users.modal.subtitle') }}</small>
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">×</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.login_id') }}</label>
                            <input type="text" id="login_id" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="login_id-error" class="invalid-feedback">This field is required.</small>
                        </div>
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.name') }}</label>
                            <input type="text" id="name" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="name-error" class="invalid-feedback">This field is required.</small>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.email') }}</label>
                            <input type="text" id="email" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="email-error" class="invalid-feedback">This field is required.</small>
                        </div>
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.gender') }}</label>
                            <select id="gender" class="form-control mr-sm-2 mb-2 mb-sm-0">
                                @foreach (g_enum('UserGenderData') as $value => $data)
                                    <option value="{{ $value }}">{{ $data[0] }}</option>
                                @endforeach
                            </select>
                            <small id="gender-error" class="invalid-feedback">This field is required.</small>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.birthday') }}</label>
                            <input type="text" id="birthday" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="birthday-error" class="invalid-feedback">This field is required.</small>
                        </div>
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.mobile') }}</label>
                            <input type="text" id="mobile" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="mobile-error" class="invalid-feedback">This field is required.</small>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.address') }}</label>
                            <input type="text" id="address" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="address-error" class="invalid-feedback">This field is required.</small>
                        </div>
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.status') }}</label>
                            <select id="status" class="form-control mr-sm-2 mb-2 mb-sm-0">
                                @foreach (g_enum('StatusData') as $value => $data)
                                    <option value="{{ $value }}">{{ $data[0] }}</option>
                                @endforeach
                            </select>
                            <small id="status-error" class="invalid-feedback">This field is required.</small>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col">
                            <label class="form-label">{{ trans('users.table.password') }}</label>
                            <input type="password" id="password" class="form-control mr-sm-2 mb-2 mb-sm-0">
                            <small id="password-error" class="invalid-feedback">This field is required.</small>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-light" data-dismiss="modal"><i class="fa fa-remove"></i>&nbsp;{{ trans('ui.button.cancel') }}</button>
                    <button type="button" class="btn btn-primary" id="modal-btn-submit"><i class="fa fa-plus"></i>&nbsp;{{ trans('ui.button.add') }}</button>
                </div>
            </form>
        </div>
    </div>
@endsection


@section('scripts')
    <script src="{{ cAsset('vendor/moment/moment.js') }}"></script>
    <script src="{{ cAsset('vendor/datatables/datatables.js') }}"></script>
    <script src="{{ cAsset('vendor/daterangepicker/daterangepicker.min.js') }}"></script>
    <script src="{{ cAsset("js/users-list.js") }}"></script>

    <script>
        function deleteRecord(id) {
            bootbox.confirm({
                message: "{{ trans('ui.alert.ask_delete') }}",
                buttons: {
                    cancel: {
                        className: 'btn btn-light',
                        label: '<i class="fa fa-times"></i> {{ trans('ui.button.cancel') }}'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> {{ trans('ui.button.confirm') }}'
                    }
                },
                callback: function(result) {
                    if (result) {
                        $.ajax({
                            url: BASE_URL + 'ajax/users/delete',
                            type: 'POST',
                            data: {
                                'id': id,
                            },
                            success: function(result) {
                                listTable.ajax.reload();
                            },
                            error: function(err) {
                                bootbox.alert("{{ trans('ui.alert.delete_failed') }}");
                                console.log(err);
                            }
                        });
                    }
                }
            });
        }

        function initTable() {
            listTable = $('#users-list').DataTable({
                processing: true,
                serverSide: true,
                searching: true,
                ajax: {
                    url: BASE_URL + 'ajax/users/search',
                    type: 'POST',
                },
                columnDefs: [{
                    targets: [0, 11],
                    orderable: false,
                    searchable: false
                }],
                order: [1, 'asc'],
                lengthMenu: [[10, 25, 50, 100, 1000, 2500, -1], [10, 25, 50, 100, 1000, 2500, "全部"]],
                columns: [
                    {data: null},
                    {data: 'id', className: "text-center"},
                    {data: 'login_id', className: "text-center"},
                    {data: 'name', className: "text-center"},
                    {data: 'email', className: "text-center"},
                    {data: 'gender', className: "text-center"},
                    {data: 'birthday', className: "text-center"},
                    {data: 'mobile', className: "text-center"},
                    {data: 'address', className: "text-center"},
                    {data: 'status', className: "text-center"},
                    {data: 'created_at', className: "text-center"},
                    {data: null, className: "text-center"},
                ],
                createdRow: function (row, data, index) {
                    var pageInfo = listTable.page.info();

                    // *********************************************************************
                    $('td', row).eq(0).html('').append(
                        '<input name="checkItem" type="checkbox" value="' + data['id'] + '">'
                    );

                    // Index
                    $('td', row).eq(1).html('').append(
                        '<span>' + (pageInfo.page * pageInfo.length + index + 1) + '</span>'
                    );

                    $('td', row).eq(5).html('').append(
                        '<span class="text-white badge badge-glow badge-' + UserGenderData[data['gender']][1] + '">' + UserGenderData[data['gender']][0] + '</span>'
                    );

                    $('td', row).eq(9).html('').append(
                        '<span class="text-white badge badge-glow badge-' + StatusData[data['status']][1] + '">' + StatusData[data['status']][0] + '</span>'
                    );

                    $('td', row).eq(11).html('').append(
                        '<a class="btn btn-icon btn-icon-rounded-circle text-primary btn-flat-primary user-tooltip" href="javascript:editUser(' +  data["id"] + ');" title="' + $('#edit-caption').val() + '">'
                        + '<i class="fa fa-edit"></i></a>' +
                        '<a class="btn btn-icon btn-icon-rounded-circle text-danger btn-flat-danger user-tooltip" onclick="deleteRecord(' +  data["id"] + ')" title="' + $('#delete-caption').val() +'">'
                        + '<i class="fa fa-remove"></i></a>'
                    );

                    if (data['loginned'] == '{{ USER_LOGINNED }}') {
                        $(row).addClass('bg-loginned');
                    }
                },
            });
        }

        $('#btn-add').on('click', function() {
            $('#edit-id').val(0);
            $('#modal-title').html('{{ trans('users.add_title') }}');
            $('#login_id').val('');
            $('#name').val('');
            $('#email').val('');
            $('#gender').val('{{ USER_GENDER_MALE }}');
            $('#birthday').val('');
            $('#mobile').val('');
            $('#address').val('');
            $('#status').val('{{ STATUS_ACTIVE }}');
            $('#password').val('');
            $('#modal-btn-submit').html('<i class="fa fa-plus"></i>&nbsp;' + '{{ trans('ui.button.add') }}');

            $('#modal-users').modal('show');
        });

        function editUser(id) {
            $.ajax({
                url: BASE_URL + 'ajax/users/getInfo',
                type: 'POST',
                data: {
                    id: id,
                },
                success: function(result) {
                    $('#edit-id').val(result['id']);
                    $('#modal-title').html('{{ trans('users.edit_title') }}' + '(' + result['name'] + ')');
                    $('#login_id').val(result['login_id']);
                    $('#name').val(result['name']);
                    $('#email').val(result['email']);
                    $('#gender').val(result['gender']);
                    $('#birthday').val(result['birthday']);
                    $('#mobile').val(result['mobile']);
                    $('#address').val(result['address']);
                    $('#status').val(result['status']);
                    $('#password').val('');
                    $('#modal-btn-submit').html('<i class="fa fa-save"></i>&nbsp;' + '{{ trans('ui.button.update') }}');

                    $('#modal-users').modal('show');
                },
                error: function(err) {
                    console.log(err);
                },
            });
        }

        $('#btn-activate-all').on('click', function() {
            var selected = [];
            var checkboxes = document.getElementsByName('checkItem');
            for (var i=0; i<checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    selected.push(checkboxes[i].value);
                }
            }

            bootbox.confirm({
                message: "{{ trans('ui.alert.ask_activate') }}",
                buttons: {
                    cancel: {
                        className: 'btn btn-light',
                        label: '<i class="fa fa-times"></i> {{ trans('ui.button.cancel') }}'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> {{ trans('ui.button.confirm') }}'
                    }
                },
                callback: function(result) {
                    if (result) {
                        $.ajax({
                            url: BASE_URL + 'ajax/users/activateAll',
                            type: 'POST',
                            data: {
                                ids: selected,
                            },
                            success: function(result) {
                                listTable.ajax.reload();
                                $('#btn-activate-all').prop('disabled', true);
                                $('#btn-bann-all').prop('disabled', true);
                                $('#checkPageItemAll').prop('checked', false);
                            },
                            error: function(err) {
                                bootbox.alert("{{ trans('ui.alert.op_failed') }}");
                                console.log(err);
                            }
                        });
                    }
                }
            });
        });

        $('#btn-bann-all').on('click', function() {
            var selected = [];
            var checkboxes = document.getElementsByName('checkItem');
            for (var i=0; i<checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    selected.push(checkboxes[i].value);
                }
            }

            bootbox.confirm({
                message: "{{ trans('ui.alert.ask_bann') }}",
                buttons: {
                    cancel: {
                        className: 'btn btn-light',
                        label: '<i class="fa fa-times"></i> {{ trans('ui.button.cancel') }}'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> {{ trans('ui.button.confirm') }}'
                    }
                },
                callback: function(result) {
                    if (result) {
                        $.ajax({
                            url: BASE_URL + 'ajax/users/bannAll',
                            type: 'POST',
                            data: {
                                ids: selected,
                            },
                            success: function(result) {
                                listTable.ajax.reload();
                                $('#btn-activate-all').prop('disabled', true);
                                $('#btn-bann-all').prop('disabled', true);
                                $('#checkPageItemAll').prop('checked', false);
                            },
                            error: function(err) {
                                bootbox.alert("{{ trans('ui.alert.op_failed') }}");
                                console.log(err);
                            }
                        });
                    }
                }
            });
        });
    </script>
@endsection
