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

<body class="vertical-layout vertical-menu-modern semi-dark-layout 2-columns  navbar-floating footer-static  " data-open="click" data-menu="vertical-menu-modern" data-col="2-columns" data-layout="semi-dark-layout" style="font-family:'メイリオ', 'Meiryo', sans-serif; !important">
<?php $user = Auth::user(); ?>
<!-- BEGIN: Header-->
<nav class="header-navbar navbar-expand-lg navbar navbar-with-menu bg-success floating-nav navbar-light navbar-shadow">
    <div class="navbar-wrapper">
        <div class="navbar-container content">
            <div class="navbar-collapse" id="navbar-mobile">
                <div class="mr-auto float-left bookmark-wrapper d-flex align-items-center">
                    <ul class="nav navbar-nav">
                        <li class="nav-item mobile-menu d-xl-none mr-auto"><a class="nav-link nav-menu-main menu-toggle hidden-xs" href="#"><i class="ficon feather icon-menu"></i></a></li>
                    </ul>
                </div>
                <ul class="nav navbar-nav float-right">
                    <li class="dropdown dropdown-user nav-item"><a class="dropdown-toggle nav-link dropdown-user-link" href="#" data-toggle="dropdown">
                            <div class="user-nav d-sm-flex d-none"><span class="user-name text-bold-600">{{ $user->login_id }}</span><span class="user-status">{{ $user->name }}</span></div><span><img class="round" src="{{ cAsset('/') }}/uploads/{{ (!isset($user->avatar) || $user->avatar == '') ? '_none.png' : $user->avatar }}" alt="avatar" height="40" width="40"></span>
                        </a>
                        <div class="dropdown-menu dropdown-menu-right"><a class="dropdown-item" href="{{ route('staff.edit') }}?id={{ $user->id }}"><i class="feather icon-user"></i> {{ trans('ui.topbar.profile') }}</a>
                            <div class="dropdown-divider"></div><a class="dropdown-item" href="{{ route('logout') }}"><i class="feather icon-power"></i> {{ trans('ui.topbar.logout') }}</a>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</nav>
<!-- END: Header-->


<!-- BEGIN: Main Menu-->
<div class="main-menu menu-fixed menu-dark menu-accordion menu-shadow" data-scroll-to-active="true">
    <div class="navbar-header">
        <ul class="nav navbar-nav flex-row">
            <li class="nav-item mr-auto"><a class="navbar-brand" href="{{ route('home') }}">
                    <h2 class="brand-text text-success mb-0">{{ env('APP_NAME') }}</h2>
                </a></li>
        </ul>
    </div>
    <div class="shadow-bottom"></div>
    <?php
        $routeName = Route::currentRouteName();
        $userId = app('request')->input('user_id');
    ?>
    <div class="main-menu-content">
        <ul class="navigation navigation-main" id="main-menu-navigation" data-menu="menu-navigation">
            <li class=" nav-item {{ strpos($routeName, 'users') === 0 ? 'active' : '' }}">
                <a href="{{ route('users') }}"><i class="feather icon-users"></i><span class="menu-title" data-i18n="Users">{{ trans('ui.sidebar.users') }}</span></a>
            </li>
            <li class=" nav-item {{ strpos($routeName, 'signals') === 0 ? 'active' : '' }}">
                <a href="{{ route('signals') }}"><i class="feather icon-zap"></i><span class="menu-title" data-i18n="Signals">{{ trans('ui.sidebar.signals') }}</span></a>
            </li>
        </ul>
    </div>
</div>
<!-- END: Main Menu-->

<!-- BEGIN: Content-->
<div class="app-content content">
    <div class="content-overlay"></div>
    <div class="header-navbar-shadow"></div>
    <div class="content-wrapper">
        <div class="content-header row">
            <div class="content-header-left col-md-9 col-12 mb-2">
                <div class="row breadcrumbs-top">
                    <div class="col-12">
                        <h2 class="content-header-title float-left mb-0">@yield('title')</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="content-body">
            @yield('contents')
        </div>
    </div>
</div>
<!-- END: Content-->

<div class="sidenav-overlay"></div>
<div class="drag-target"></div>

<!-- BEGIN: Footer-->
<footer class="footer footer-static footer-light">
    <p class="clearfix blue-grey lighten-2 mb-0"><span class="float-md-left d-block d-md-inline-block mt-25">COPYRIGHT &copy; <a class="text-bold-800 grey darken-2" href="{{ route('home') }}" target="_blank">{{ env('APP_NAME') }} Co. Ltd. </a>All rights Reserved</span>
        <button class="btn btn-primary btn-icon scroll-top" type="button"><i class="feather icon-arrow-up"></i></button>
    </p>
</footer>
<!-- END: Footer-->


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
