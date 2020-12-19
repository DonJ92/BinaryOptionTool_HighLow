@extends('layouts.afterlogin')

@section('title', trans('signals.title'))

@section('styles')
    <link href="{{ cAsset('vendor/datatables/datatables.css') }}" rel="stylesheet">
    <link href="{{ cAsset('vendor/daterangepicker/daterangepicker.css') }}" rel="stylesheet">
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
                                    <label class="form-label">{{ trans('signals.table.source') }}</label>
                                    <input type="text" id="filter-source" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('signals.table.symbol') }}</label>
                                    <input type="text" id="filter-symbol" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('signals.table.cmd') }}</label>
                                    <select id="filter-cmd" class="form-control">
                                        <option value="">{{ trans('ui.search.any') }}</option>
                                        @foreach (g_enum('SignalTypeData') as $index => $status)
                                            <option value="{{ $index }}">{{ $status[0] }}</option>
                                        @endforeach
                                    </select>
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('signals.table.status') }}</label>
                                    <select id="filter-status" class="form-control">
                                        <option value="">{{ trans('ui.search.any') }}</option>
                                        @foreach (g_enum('SignalStatusData') as $index => $status)
                                            <option value="{{ $index }}">{{ $status[0] }}</option>
                                        @endforeach
                                    </select>
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('signals.table.occurred_at') }}</label>
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

        <?php
            echo '<script>';
            echo 'var SignalTypeData = ' . json_encode(g_enum('SignalTypeData')) . ';';
            echo 'var SignalStatusData = ' . json_encode(g_enum('SignalStatusData')) . ';';
            echo '</script>';
        ?>

        <div class="card">
            <div class="card-content">
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="signals-list" class="table">
                            <thead>
                            <tr>
                                <th>{{ trans('signals.table.no') }}</th>
                                <th>{{ trans('signals.table.source') }}</th>
                                <th>{{ trans('signals.table.symbol') }}</th>
                                <th>{{ trans('signals.table.cmd') }}</th>
                                <th>{{ trans('signals.table.status') }}</th>
                                <th>{{ trans('signals.table.copied') }}</th>
                                <th>{{ trans('signals.table.occurred_at') }}</th>
                                <th>{{ trans('signals.table.actions') }}</th>
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
@endsection


@section('scripts')
    <script src="{{ cAsset('vendor/moment/moment.js') }}"></script>
    <script src="{{ cAsset('vendor/datatables/datatables.js') }}"></script>
    <script src="{{ cAsset('vendor/daterangepicker/daterangepicker.min.js') }}"></script>
    <script src="{{ cAsset("js/signals-list.js") }}"></script>

    <script>
        function initTable() {
            listTable = $('#signals-list').DataTable({
                processing: true,
                serverSide: true,
                searching: true,
                ajax: {
                    url: BASE_URL + 'ajax/signals/search',
                    type: 'POST',
                },
                columnDefs: [],
                order: [6, 'desc'],
                columns: [
                    {data: 'id', className: "text-center"},
                    {data: 'source', className: "text-center"},
                    {data: 'symbol', className: "text-center"},
                    {data: 'cmd', className: "text-center"},
                    {data: 'status', className: "text-center"},
                    {data: 'copied', className: "text-center"},
                    {data: 'created_at', className: "text-center"},
                    {data: null, className: "text-center"},
                ],
                createdRow: function (row, data, index) {
                    var pageInfo = listTable.page.info();

                    // *********************************************************************
                    // Index
                    $('td', row).eq(0).html('').append(
                        '<span>' + (pageInfo.page * pageInfo.length + index + 1) + '</span>'
                    );

                    $('td', row).eq(3).html('').append(
                        '<span class="text-white badge badge-glow badge-' + SignalTypeData[data['cmd']][1] + '">' + SignalTypeData[data['cmd']][0] + '</span>'
                    );
                    $('td', row).eq(4).html('').append(
                        '<span class="text-white badge badge-glow badge-' + SignalStatusData[data['status']][1] + '">' + SignalStatusData[data['status']][0] + '</span>'
                    );
                    $('td', row).eq(7).html('').append(
                        '<a target="_blank" class="btn btn-icon btn-icon-rounded-circle text-primary btn-flat-primary user-tooltip" href="' + '{{ route('signals.detail') }}' + '?id=' +  data["id"] + '" title="' + '{{ trans('ui.button.detail') }}' + '">'
                        + '<i class="fa fa-edit"></i></a>'
                    );
                },
            });
        }
    </script>
@endsection
