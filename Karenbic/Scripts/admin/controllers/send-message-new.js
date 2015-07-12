App.controller('NewSendMessageController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.customerGroupFilter = false;
        $scope.customerGroups = [];
        $scope.selectedCustomerGroups = [];

        $scope.customerFilter = false;
        $scope.customers = [];
        $scope.selectedCustomers = [];

        $scope.froalaOptions = {
            buttons: ["bold", "italic", "underline", "strikeThrough", "fontFamily", "fontSize", "color",
                    "sep",
                    "formatBlock", "align", "insertOrderedList", "insertUnorderedList", "outdent", "indent", "selectAll",
                    "sep",
                    "insertHorizontalRule", "createLink", "table", "insertImage", "insertVideo", "undo", "redo", "html"],
            inlineMode: false,
            inverseSkin: true,
            allowedImageTypes: ["jpeg", "jpg", "png"],
            height: 300,
            language: "fa",
            direction: "rtl",
            fontList: ["Tahoma, Geneva", "Arial, Helvetica", "Impact, Charcoal"],
            spellcheck: true,
            plainPaste: true,
            borderColor: '#00008b',
            enableScript: false
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