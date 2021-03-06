﻿App.controller('NewSendMessageController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.customerGroupFilter = false;
        $scope.customerGroups = [];
        $scope.selectedCustomerGroups = [];

        $scope.customerFilter = false;
        $scope.customers = [];
        $scope.selectedCustomers = [];

        $scope.editorOptions = {
            language: 'fa',
            uiColor: '#f2f2f2',
            resize_enabled: false,
            height: 290,
            enterMode: CKEDITOR.ENTER_BR,
            font_names: 'Arial;BNAZANB;BNazanin;IranianSerifWeb;NexaBold;Thoma;yekan;',// + config.font_names,
            toolbar: [
	            { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'] },
	            { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
	            { name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
	            //{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
	            '/',
	            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
	            '/',
	            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
	            { name: 'colors', items: ['TextColor', 'BGColor'] },
	            { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
	            { name: 'others', items: ['-'] }
            ],
            toolbarGroups: [
	            { name: 'document', groups: ['mode', 'document', 'doctools'] },
	            { name: 'clipboard', groups: ['clipboard', 'undo'] },
	            { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
	            { name: 'forms' },
	            '/',
	            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
	            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
	            { name: 'links' },
	            { name: 'insert' },
	            '/',
	            { name: 'styles' },
	            { name: 'colors' },
	            { name: 'tools' },
	            { name: 'others' },
	            { name: 'about' }
            ],
            ImageBrowser: true,
            ImageBrowserURL: '/Vendors/ckeditor/finder/index.html',
            filebrowserImageBrowseUrl: '/Vendors/ckeditor/finder/index.html',
            filebrowserFlashBrowseUrl: '/Vendors/ckeditor/finder/index.html',
            filebrowserUploadUrl: '/Vendors/ckeditor/finder/connectors/ashx/filemanager.ashx',
            filebrowserImageUploadUrl: '/Vendors/ckeditor/finder/connectors/apsx/filemanager.ashx',
            filebrowserFlashUploadUrl: '/Vendors/ckeditor/finder/connectors/apsx/filemanager.ashx'
        };

        $scope.message = {
            Title: '',
            Text: ''
        };

        $scope.fetchCustomerGroups = function () {
            $scope.fetchCustomerGroupsLoading = true;
            $http.get(baseUri + 'CustomerGroup/Get')
            .success(function (data, status, headers, config) {
                $scope.customerGroups = data.Data;
                $scope.fetchCustomerGroupsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCustomerGroupsLoading = false;
            });
        };

        $scope.fetchCustomers = function () {
            $scope.fetchCustomersLoading = true;
            $http.get(baseUri + 'SendMessage/New_GetCustomers', {
                params: {
                    customerGroupId: _.map($scope.selectedCustomerGroups, function (item) {
                        return item.Id;
                    })
                }
            })
            .success(function (data, status, headers, config) {
                $scope.customers = data.Data;
                $scope.fetchCustomersLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCustomersLoading = false;
            });
        };

        $scope.$watch('selectedCustomerGroups', function (newValue, oldValue) {
            if (newValue != null && newValue.length > 0) {
                $scope.fetchCustomers();
            }
            else {
                $scope.selectedCustomers = [];
            }
        });

        $scope.send = function () {
            if ($scope.sendMessageForm.$invalid) return;

            $scope.sendLoading = true;
            $http.post(baseUri + 'SendMessage/New',
            {
                title: $scope.message.Title,
                text: $scope.message.Text,
                isCustomerGroupFilter: $scope.customerGroupFilter,
                customerGroupsId: _.map($scope.selectedCustomerGroups, function (item) {
                    return item.Id;
                }),
                isCustomerFilter: $scope.customerFilter,
                customersId: _.map($scope.selectedCustomers, function (item) {
                    return item.Id;
                })
            }).
            success(function (data, status, headers, config) {
                $scope.message = {
                    Title: '',
                    Text: ''
                };
                $scope.customerGroupFilter = false;
                $scope.selectedCustomerGroups = [];
                $scope.customerFilter = false;
                $scope.selectedCustomers = [];
                $scope.sendLoading = false;
                toaster.pop('success', "پیام شما با موفقیت ارسال شد");
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.sendLoading = false;
            });
        };

        //init
        $scope.fetchCustomerGroups();
    }]);