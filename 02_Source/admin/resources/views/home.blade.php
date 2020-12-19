@extends('layouts.afterlogin')

@section('styles')
    <link rel="stylesheet" type="text/css" href="{{ cAsset("app-assets/vendors/css/charts/apexcharts.css") }}">
@endsection

@section('contents')
@endsection

@section('scripts')
    <script src="{{ cAsset("app-assets/vendors/js/charts/apexcharts.min.js") }}"></script>
    <script src="{{ cAsset("app-assets/js/scripts/pages/dashboard-analytics.js") }}"></script>
@endsection
