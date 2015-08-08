App.controller('PrintOrderPriceController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.formGroup = {};
        $scope.formGroups = [];
        $scope.form = {};
        $scope.forms = [];
        $scope.prices = [];

        //Fetch Form Groups
        $scope.fetchFormGroups = function () {
            $scope.fetchformGroupsLoading = true;

            $http.get(baseUri + 'FormGroup/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2
                }
            })
            .success(function (data, status, headers, config) {
                $scope.formGroups = data.Data;
                if ($scope.formGroups != null && $scope.formGroups.length > 0)
                    $scope.formGroup = $scope.formGroups[0];
                $scope.fetchformGroupsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchformGroupsLoading = false;
            });
        };
        $scope.fetchFormGroups();

        //Fetch Forms
        $scope.$watch('formGroup', function (newValue, oldValue) {
            $scope.fetchForms();
        });

        $scope.fetchForms = function () {
            if ($scope.formGroup.Id == null) return;

            $scope.fetchFormsLoading = true;

            $http.get(baseUri + 'Form/GetByGroup', {
                params: {
                    groupId: $scope.formGroup.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.forms = data;
                if ($scope.forms != null && $scope.forms.length > 0)
                    $scope.form = $scope.forms[0];
                $scope.fetchFormsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchFormsLoading = false;
            });
        };

        //Fetch Prices
        $scope.$watch('form', function (newValue, oldValue) {
            $scope.fetchPrices();
        });

        $scope.fetchPrices = function () {
            if ($scope.form.Id == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PrintOrderPrice/Get', {
                params: {
                    formId: $scope.form.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.prices = data;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchLoading = false;
            });
        };

        //Add Price
        $scope.openAddPriceDialog = function () {
            if ($scope.form.Id == null) return;

            var modalInstance = $modal.open({
                templateUrl: '/AddPrice.html',
                controller: AddPriceCtrl,
                size: 'xs',
                resolve: {
                    formGroup: function () {
                        return _.clone($scope.formGroup);
                    },
                    form: function () {
                        return _.clone($scope.form);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.fetchPrices();
            }, function () {
            });
        };

        var AddPriceCtrl = ['$scope', '$http', '$modalInstance', 'formGroup', 'form', function ($scope, $http, $modalInstance, formGroup, form) {

            $scope.formGroup = formGroup;
            $scope.form = form;
            $scope.fields = [];
            $scope.newPrice = {
                Priority: 0,
                PrintPrice: 0,
                PackingPrice: 0
            };

            $scope.fetchFields = function () {
                $scope.fetchFieldsLoading = true;
                $http.get(baseUri + 'OrderPrice/GetFields', {
                    params: {
                        id: $scope.form.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.fields = _.sortBy(data, function (item) {
                        return item.mobile_position.row;
                    });
                    $scope.fetchFieldsLoading = false;
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                    }
                    $scope.fetchFieldsLoading = false;
                });
            };

            /*=-=-=-=-= Start Validate Field =-=-=-=-=*/

            $scope.validateFields = function () {
                var valide = true;

                _.each($scope.fields, function (item) {
                    switch (item.type) {
                        case 2:
                            if ($scope.validateForm_Nummeric(item) == false) {
                                valide = false;
                            }
                            break;

                        case 5:
                            break;

                        case 8:
                            if ($scope.validateForm_DropDown(item) == false) {
                                valide = false;
                            }
                            break;

                        case 9:
                            if ($scope.validateForm_MultipleChoice(item) == false) {
                                valide = false;
                            }
                            break;

                        case 10:
                            break;
                    }
                });

                return valide;
            };

            $scope.validateForm_Nummeric = function (item) {
                var valide = true;
                if (item.data.isRequired == true && 
                    (isNaN(item.minValue) == true || isNaN(item.maxValue) == true)) {
                    valide = false;
                }
                if (item.data.limits == true) {
                    if (isNaN(item.minValue) == true) valide = false;
                    else {
                        if (item.minValue < item.data.min) valide = false;
                        if (item.minValue > item.data.max) valide = false;
                    }

                    if (isNaN(item.maxValue) == true) valide = false;
                    else {
                        if (item.maxValue < item.data.min) valide = false;
                        if (item.maxValue > item.data.max) valide = false;
                    }
                }
                return valide;
            };

            $scope.validateForm_DropDown = function (item) {
                var valide = true;
                if (item.data.isRequired == true && item.value == null) {
                    valide = false;
                }
                return valide;
            };

            $scope.validateForm_MultipleChoice = function (item) {
                var valide = true;
                if (item.data.isRequired == true && item.value == null) {
                    valide = false;
                }
                return valide;
            };

            /*=-=-=-=-= End Validate Field =-=-=-=-=*/

            /*=-=-=-=-= Start Send Data =-=-=-=-=*/
            $scope.fromSumbited = false;

            $scope.send = function () {
                $scope.fromSumbited = true;
                if (addPriceForm.$invalid == true && $scope.validateForm() == false) return;

                $scope.addLoading = true;
                $http.post(baseUri + 'PrintOrderPrice/Add',
                {
                    formId: $scope.form.Id,
                    numerics: $scope.getFieldsValue(2),
                    checkboxs: $scope.getFieldsValue(5),
                    dropDowns: $scope.getFieldsValue(8),
                    radioButtonGroups: $scope.getFieldsValue(9),
                    checkBoxGroups: $scope.getFieldsValue(10),
                    priority: $scope.newPrice.Priority,
                    printPrice: $scope.newPrice.PrintPrice,
                    packingPrice: $scope.newPrice.PackingPrice
                }).
                success(function (data, status, headers, config) {
                    $scope.addLoading = false;
                    if (data.Id != -1) {
                        toaster.pop('success', "اطلاعات با موفقیت ثبت گردید");
                        $modalInstance.close(data.Id);
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    $scope.addLoading = false;
                });
            };

            $scope.getFieldsValue = function (type) {
                var filters = _.filter($scope.fields, function (item) { return item.type == type; });

                var values = _.map(filters, function (item) {
                    switch (item.type) {
                        case 2:
                            return {
                                FieldId: item.data.id,
                                MinValue: item.minValue,
                                MaxValue: item.maxValue
                            };
                            break;

                        case 5:
                            return {
                                FieldId: item.data.id,
                                Value: item.value
                            };
                            break;

                        case 8:
                        case 9:
                            return {
                                FieldId: item.data.id,
                                Value: item.value.id
                            };
                            break;

                        case 10:
                            var filters = _.filter(item.data.items, function (val) { return val.value == true; });

                            return {
                                FieldId: item.data.id,
                                Values: _.map(filters, function (val) { return val.id; })
                            };
                            break;
                    }
                });

                return values;
            };
            /*=-=-=-=-= End Send Data =-=-=-=-=*/

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            //init
            $scope.fetchFields();
        }];

        //Remove
        $scope.remove = function (index) {
            ngDialog.open({
                template: 'removeDialog.html',
                showClose: false,
                controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                    $scope.close = function () {
                        $scope.closeThisDialog(0);
                    };
                    $scope.confirm = function () {
                        $scope.closeThisDialog(1);
                    };
                }],
                preCloseCallback: function (value) {
                    if (value != 1) return true;

                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'PrintOrderPrice/Remove',
                    {
                        id: $scope.prices[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchPrices();
                        }
                        else {
                            toaster.pop('error', "امکان حذف اطلاعات وجود ندارد");
                        }
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.fetchLoading = false;
                    });
                    return true;
                }
            });
        };
    }]);