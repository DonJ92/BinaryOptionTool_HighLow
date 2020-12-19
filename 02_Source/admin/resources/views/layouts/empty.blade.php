<!DOCTYPE html>
<html class="loading" lang="en" data-textdirection="ltr">
<!-- BEGIN: Head-->

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
    <meta name="description" content="Vuexy admin is super flexible, powerful, clean &amp; modern responsive bootstrap 4 admin template with unlimited possibilities.">
    <meta name="keywords" content="admin template, Vuexy admin template, dashboard template, flat admin template, responsive admin template, web app">
    <meta name="author" content="PIXINVENT">
    <title>{{ env('APP_NAME') }}&nbsp;|&nbsp;@yield('title')</title>
    <link rel="apple-touch-icon" href="{{ cAsset("app-assets/images/ico/apple-icon-120.png") }}">
    <link rel="shortcut icon" type="image/x-icon" href="{{ cAsset("favicon.png") }}">
    <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600" rel="stylesheet">

    <!-- BEGIN: Vendor CSS-->
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/vendors/css/vendors.min.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/vendors/css/extensions/tether-theme-arrows.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/vendors/css/extensions/tether.min.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/vendors/css/extensions/shepherd-theme-default.css") }}">
    <!-- END: Vendor CSS-->

    <!-- BEGIN: Theme CSS-->
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/bootstrap.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/bootstrap-extended.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/colors.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/components.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/themes/dark-layout.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/themes/semi-dark-layout.css") }}">

    <!-- BEGIN: Page CSS-->
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/core/menu/menu-types/vertical-menu.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/core/colors/palette-gradient.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/pages/dashboard-analytics.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/pages/card-analytics.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/css/plugins/tour/tour.css") }}">
    <link rel="stylesheet" type="text/css" href="{{ cAsset('app-assets/css/pages/app-user.css') }}">
    <!-- END: Page CSS-->

    @yield('styles')
    <style>
        .dataTables_filter {
            display:none;
        }
    </style>

</head>
<!-- END: Head-->

<!-- BEGIN: Body-->

<body class="semi-dark-layout" data-open="click" data-menu="vertical-menu-modern" data-col="2-columns" data-layout="semi-dark-layout">
<!-- BEGIN: Content-->
@yield('contents')
<!-- END: Content-->

<!-- BEGIN: Vendor JS-->
<script src="{{ cAsset("app-assets/vendors/js/vendors.min.js") }}"></script>
<!-- BEGIN Vendor JS-->

<!-- BEGIN: Page Vendor JS-->
<!--<script src="{{ cAsset("app-assets/vendors/js/charts/apexcharts.min.js") }}"></script>-->
<script src="{{ cAsset("app-assets/vendors/js/extensions/tether.min.js") }}"></script>
<!--<script src="{{ cAsset("app-assets/vendors/js/extensions/shepherd.min.js") }}"></script>-->
<!-- END: Page Vendor JS-->

<!-- BEGIN: Theme JS-->
<script src="{{ cAsset("app-assets/js/core/app-menu.js") }}"></script>
<script src="{{ cAsset("app-assets/js/core/app.js") }}"></script>
<script src="{{ cAsset("app-assets/js/scripts/components.js") }}"></script>
<!-- END: Theme JS-->

<!-- BEGIN: Page JS-->
<script src="{{ cAsset('vendor/bootbox/bootbox.js') }}"></script>
<script src="{{ cAsset('js/__common.js') }}"></script>
<!-- END: Page JS-->

<script>
    var PUBLIC_URL = '{{ cAsset('/') . '/' }}';
    var BASE_URL = PUBLIC_URL;
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': '{{ csrf_token() }}'
        }
    });
</script>

@yield('scripts')

</body>
<!-- END: Body-->

</html>
