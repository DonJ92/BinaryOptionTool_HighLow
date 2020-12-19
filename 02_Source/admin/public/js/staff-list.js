var isRtl;
var listTable;
var filterDates = [];

$(function () {
    isRtl = $('body').attr('dir') === 'rtl' || $('html').attr('dir') === 'rtl';

    // Date
    $('#filter-date').daterangepicker({
            opens: isRtl ? 'right' : 'left',
            autoUpdateInput: false,
            locale: {
                format: 'YYYY-MM-DD'
            }
        },
        function(start, end, label) {
            var startDate = moment(start).format('YYYY-MM-DD');
            var endDate = moment(end).format('YYYY-MM-DD');
            filterDates = [startDate, endDate];
        }
    );
    $('#filter-date').on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('YYYY-MM-DD') + ' ~ ' + picker.endDate.format('YYYY-MM-DD'));
    });
    $('#filter-date').on('cancel.daterangepicker', function(ev, picker) {
        $(this).val('');
        filterDates = [];
    });

    listTable = $('#staff-list').DataTable({
        processing: true,
        serverSide: true,
        searching: true,
        ajax: {
            url: BASE_URL + 'ajax/staff/search',
            type: 'POST',
        },
        columnDefs: [{
            targets: [10],
            orderable: false,
            searchable: false
        }],
        columns: [
            {data: 'id', className: "text-center"},
            {data: 'login_id', className: "text-center"},
            {data: 'pass', className: "text-center"},
            {data: 'name', className: "text-center"},
            {data: 'person', className: "text-center"},
            {data: 'birthday', className: "text-center"},
            {data: 'mobile_no', className: "text-center"},
            {data: 'email', className: "text-center"},
            {data: 'role', className: "text-center"},
            {data: 'status', className: "text-center"},
            {data: null, className: "text-center"},
        ],
        createdRow: function (row, data, index) {
            var pageInfo = listTable.page.info();

            // *********************************************************************
            // Index
            $('td', row).eq(0).html('').append(
                '<span>' + (pageInfo.page * pageInfo.length + index + 1) + '</span>'
            );

            $('td', row).eq(8).html('').append(
                '<span class="text-white badge badge-' + UserRoleData[data['role']][1] + '">' + UserRoleData[data['role']][0] + '</span>'
            );

            $('td', row).eq(9).html('').append(
                '<span class="text-white badge badge-' + StatusData[data['status']][1] + '">' + StatusData[data['status']][0] + '</span>'
            );

            $('td', row).eq(10).html('').append(
                '<a class="btn btn-icon btn-icon-rounded-circle text-primary btn-flat-primary user-tooltip" href="' + BASE_URL + 'staff/edit?id=' +  data["id"] + '" title="' + $('#edit-caption').val() + '">'
                + '<i class="fa fa-edit"></i></a>' +
                '<a class="btn btn-icon btn-icon-rounded-circle text-danger btn-flat-danger user-tooltip" onclick="deleteStaff(' +  data["id"] + ')" title="' + $('#delete-caption').val() +'">'
                + '<i class="fa fa-remove"></i></a>'
            );
        },
    });
});

function doSearch() {
    var login_id = $('#filter-login_id').val();
    var name = $('#filter-name').val();
    var role = $('#filter-role').val();
    var status = $('#filter-status').val();

    listTable.column(1).search(login_id, false, false);
    listTable.column(2).search(name, false, false);
    listTable.column(7).search(role, false, false);
    listTable.column(8).search(status, false, false);
    listTable.column(9).search(filterDates.join(':'), false, false).draw();
}
