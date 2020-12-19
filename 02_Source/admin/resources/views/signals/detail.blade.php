@extends('layouts.afterlogin')

@section('title', trans('copies.title'))

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
                                    <label class="form-label">{{ trans('copies.table.trader') }}</label>
                                    <input type="text" id="filter-trader" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('copies.table.status') }}</label>
                                    <select id="filter-status" class="form-control">
                                        <option value="">{{ trans('ui.search.any') }}</option>
                                        @foreach (g_enum('CopiesStatusData') as $index => $status)
                                            <option value="{{ $index }}">{{ $status[0] }}</option>
                                        @endforeach
                                    </select>
                                </div>
                                <div class="col-md">
                                    <label class="form-label">{{ trans('copies.table.ordered_at') }}</label>
                                    <input type="text" id="filter-date" class="form-control" placeholder="{{ trans('ui.search.any') }}">
                                </div>
                                <div class="col-md col-xl-2">
                                    <label class="form-label">&nbsp;</label>
                                    <button type="button" onclick="javascript:doSearch()" class="btn btn-primary btn-block">
                                        <i class="fa fa-search"></i>&nbsp;{{ trans('ui.button.search') }}
                                    </button>
                                </div>
                                <div class="col-md col-xl-2">
                                    <label class="form-label">&nbsp;</label>
                                    <button type="button" id="btn-back" class="btn btn-secondary btn-block">
                                        <i class="fa fa-arrow-left"></i>&nbsp;{{ trans('ui.button.back') }}
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
            echo 'var CopiesDemoRealData = ' . json_encode(g_enum('CopiesDemoRealData')) . ';';
            echo 'var TransModeData = ' . json_encode(g_enum('TransModeData')) . ';';
            echo 'var TransTimeUnitsData = ' . json_encode(g_enum('TransTimeUnitsData')) . ';';
            echo 'var CopiesStatusData = ' . json_encode(g_enum('CopiesStatusData')) . ';';
            echo '</script>';
        ?>

        <div class="card">
            <div class="card-content">
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="copies-list" class="table">
                            <thead>
                            <tr>
                                <th>{{ trans('copies.table.no') }}</th>
                                <th>{{ trans('copies.table.source') }}</th>
                                <th>{{ trans('copies.table.symbol') }}</th>
                                <th>{{ trans('copies.table.trader') }}</th>
                                <th>{{ trans('copies.table.demo_real') }}</th>
                                <th>{{ trans('copies.table.trans_mode') }}</th>
                                <th>{{ trans('copies.table.trans_timeunit') }}</th>
                                <th>{{ trans('copies.table.price') }}</th>
                                <th>{{ trans('copies.table.amount') }}</th>
                                <th>{{ trans('copies.table.status') }}</th>
                                <th>{{ trans('copies.table.ordered_at') }}</th>
                                <th>{{ trans('copies.table.judged_at') }}</th>
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
    <script src="{{ cAsset("js/signals-detail.js") }}"></script>

    <script>
        function initTable() {
            listTable = $('#copies-list').DataTable({
                processing: true,
                serverSide: true,
                searching: true,
                ajax: {
                    url: BASE_URL + 'ajax/signals/detail',
                    type: 'POST',
                    data: {
                        id: '{{ $signal_id }}',
                    },
                },
                columnDefs: [],
                columns: [
                    {data: 'id', className: "text-center"},
                    {data: 'source', className: "text-center"},
                    {data: 'symbol', className: "text-center"},
                    {data: 'trader', className: "text-center"},
                    {data: 'demo_real', className: "text-center"},
                    {data: 'trans_mode', className: "text-center"},
                    {data: 'trans_timeunit', className: "text-center"},
                    {data: 'order_price', className: "text-center"},
                    {data: 'order_amount', className: "text-center"},
                    {data: 'status', className: "text-center"},
                    {data: 'ordered_at', className: "text-center"},
                    {data: 'judged_at', className: "text-center"},
                ],
                createdRow: function (row, data, index) {
                    var pageInfo = listTable.page.info();

                    // *********************************************************************
                    // Index
                    $('td', row).eq(0).html('').append(
                        '<span>' + (pageInfo.page * pageInfo.length + index + 1) + '</span>'
                    );

                    $('td', row).eq(4).html('').append(
                        '<span class="text-white badge badge-glow badge-' + CopiesDemoRealData[data['demo_real']][1] + '">' + CopiesDemoRealData[data['demo_real']][0] + '</span>'
                    );
                    $('td', row).eq(5).html('').append(
                        '<span class="text-white badge badge-glow badge-' + TransModeData[data['trans_mode']][1] + '">' + TransModeData[data['trans_mode']][0] + '</span>'
                    );
                    $('td', row).eq(6).html('').append(
                        '<span class="text-white badge badge-glow badge-' + TransTimeUnitsData[data['trans_timeunit']][1] + '">' + TransTimeUnitsData[data['trans_timeunit']][0] + '</span>'
                    );
                    $('td', row).eq(7).html('').append(
                        _number_format(data['order_price'], 6) + '/' + (data['judge_price'] == 0 ? '-' : _number_format(data['judge_price'], 6))
                    )
                    $('td', row).eq(8).html('').append(
                        _number_format(data['order_amount'], 2) + '/' + _number_format(data['payout_amount'], 2)
                    );
                    $('td', row).eq(9).html('').append(
                        '<span class="text-white badge badge-glow badge-' + CopiesStatusData[data['status']][1] + '">' + CopiesStatusData[data['status']][0] + '</span>'
                    );
                },
            });
        }

        $('#btn-back').on('click', function() {
            document.location.href = '{{ route('signals') }}';
        });
    </script>
@endsection
