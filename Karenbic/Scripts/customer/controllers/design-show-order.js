App.controller('ShowDesignOrderController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$stateParams', '$upload', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $stateParams, $upload, $state) {
        $scope.orderId = $stateParams.id;
        $scope.order = {};
        $scope.designs = [];
        $scope.states = {
            None: 1,
            Accept: 2,
            Maybe: 3,
            UnAccept: 4
        };

        /*=-=-=-=-=-=-=-=-= Start Order Detail =-=-=-=-=-=-=-=-=*/
        $scope.fetchData = function () {
            $scope.fetchDataLoading = true;

            $http.get(baseUri + 'DesignOrder/Show_GetData', {
                params: {
                    id: $scope.orderId
                }
            })
            .success(function (data, status, headers, config) {
                $scope.order = data;
                $scope.fetchDataLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchDataLoading = false;
            });
        };

        $scope.showDetails = function () {
            var modalInstance = $modal.open({
                templateUrl: '/ShowDetailsContent.html',
                controller: ShowDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.order);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowDetailsCtrl = function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'Order/Details', {
                    params: {
                        orderId: $scope.order.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.values = data;
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
            $scope.fetchValues();

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        };
        /*=-=-=-=-=-=-=-=-= End Order Detail =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Designs =-=-=-=-=-=-=-=-=*/
        $scope.fetchDesigns = function () {
            $scope.fetchDesignLoading = true;

            $http.get(baseUri + 'DesignOrder/Show_GetDesigns', {
                params: {
                    id: $scope.orderId
                }
            })
            .success(function (data, status, headers, config) {
                $scope.designs = data.Data;
                $scope.fetchDesignLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchDesignLoading = false;
            });
        };
        /*=-=-=-=-=-=-=-=-= End Designs =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Checkout =-=-=-=-=-=-=-=-=*/
        $scope.showCheckouDesigntModal = function (designIndex) {
            var modalInstance = $modal.open({
                templateUrl: '/CheckoutDesignContent.html',
                controller: CheckoutDesignCtrl,
                size: 'lg',
                resolve: {
                    design: function () {
                        var design = {
                            Id: $scope.designs[designIndex].Id,
                            Description: $scope.designs[designIndex].Description,
                            PersianRegisterDate: $scope.designs[designIndex].PersianRegisterDate,
                            Time: $scope.designs[designIndex].Time,
                            IsPaidPrepayment: $scope.designs[designIndex].IsPaidPrepayment,
                            IsPaidFinal: $scope.designs[designIndex].IsPaidFinal,
                            Price: $scope.designs[designIndex].Price,
                            Prepayment: $scope.designs[designIndex].Prepayment,
                            Files: []
                        };
                        for (i = 0; i < $scope.designs[designIndex].Files.length; i++) {
                            design.Files.push(_.clone($scope.designs[designIndex].Files[i]));
                        }
                        return design;
                    },
                    order: function () {
                        return {
                            Id: $scope.order.Id,
                            Price: $scope.order.Price,
                            Prepayment: $scope.order.Prepayment,
                            IsPaidPrepayment: $scope.order.IsPaidPrepayment,
                            IsPaidFinal: $scope.order.IsPaidFinal
                        };
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.designs[designIndex].Files = result;
                $scope.designs[designIndex].IsReview = true;
            }, function () {
            });
        };

        var CheckoutDesignCtrl = function ($scope, $http, $modalInstance, design, order) {

            $scope.design = design;
            $scope.order = order;
            $scope.states = {
                None: 1,
                Accept: 2,
                Maybe: 3,
                UnAccept: 4
            };
            $scope.selectedFile = $scope.design.Files[0];
            $scope.selectedIndex = 0;
            $scope.acceptedFile = null;
            $scope.acceptedIndex = -1;

            $scope.selectFile = function (index) {
                $scope.selectedFile = $scope.design.Files[index];
                $scope.selectedIndex = index;
            };

            $scope.selectNextFile = function () {
                if ($scope.selectedIndex < $scope.design.Files.length -1) {
                    $scope.selectedIndex++;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = 0;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.selectPrevFile = function () {
                if ($scope.selectedIndex > 0) {
                    $scope.selectedIndex--;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = $scope.design.Files.length - 1;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.selectAcceptedFile = function () {
                $scope.design.Files[$scope.selectedIndex].State = $scope.states.Accept;
                $scope.acceptedFile = $scope.design.Files[$scope.selectedIndex];
                $scope.acceptedIndex = $scope.selectedIndex;
            };

            $scope.selectMaybeFile = function () {
                $scope.design.Files[$scope.selectedIndex].State = $scope.states.Maybe;
                if ($scope.acceptedIndex == $scope.selectedIndex) {
                    $scope.acceptedFile = null;
                    $scope.acceptedIndex = -1;
                }
            };

            $scope.selectUnAcceptedFile = function () {
                $scope.design.Files[$scope.selectedIndex].State = $scope.states.UnAccept;
                if ($scope.acceptedIndex == $scope.selectedIndex) {
                    $scope.acceptedFile = null;
                    $scope.acceptedIndex = -1;
                }
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            $scope.sendReview = function () {
                var firstUnReview = _.find($scope.design.Files, function (item) {
                    return item.State != $scope.states.Accept &&
                        item.State != $scope.states.Maybe &&
                        item.State != $scope.states.UnAccept;
                });

                if ($scope.acceptedFile == null &&
                    (firstUnReview != null || firstUnReview != undefined)) {
                    ngDialog.open({
                        template: '<div style="font-family:yekan;font-size:14px;">' +
                            '<p>' + 'لطفاً تمام طراح ها را بررسی نمایید' + '</p>' +
                            '<div style="text-align:left;">' +
                            '<button type="button" class="btn btn-primary" ng-click="closeThisDialog(0)">خروج</button>' +
                            '</div></div>',
                        plain: true,
                        showClose: false
                    });
                }

                else if ($scope.acceptedFile != null &&
                    ($scope.order.IsPaidPrepayment == false || $scope.order.IsPaidFinal == false)) {
                    var money = 0;
                    if ($scope.order.IsPaidPrepayment == false) {
                        money += $scope.order.Prepayment;
                    }
                    if ($scope.order.IsPaidFinal == false) {
                        money += ($scope.order.Price - $scope.order.Prepayment);
                    }

                    var separator = ",";
                    var moneyStr = escape(money).replace(new RegExp(separator, "g"), "");
                    var regexp = new RegExp("\\B(\\d{3})(" + separator + "|$)");
                    do {
                        moneyStr = moneyStr.replace(regexp, separator + "$1");
                    }
                    while (moneyStr.search(regexp) >= 0)

                    ngDialog.open({
                        template: '<div style="font-family:yekan;font-size:14px;">' +
                            '<p>' +
                            'تأیید طرح روبرو از جانب شما به منزله اتمام پروسه طراحی برای این سفارش می باشد ' +
                            'و در این بخش برای اعمال تأیید از جانب شما نیازمند پرداخت مانده حساب به مبلغ ' +
                            moneyStr +
                            ' ریال ' +
                            'می باشد' +
                            '</p>' +
                            '<p>' +
                            'لازم به ذکر است که در صورت عدم اتمام روند پرداخت به هر دلیل نظر شما در سیستم ثبت نشده و می بایست مراحا را از ابتدا انجام دهید.' +
                            '</p>' +
                            '<div style="text-align:left;">' +
                            '<button class="btn btn-warning" ng-click="close()" type="button">انصراف</button>' +
                            '<button class="btn btn-primary" ng-click="confirm()" type="button" style="margin-right:4px;">پرداخت</button>' +
                            '</div></div>',
                        plain: true,
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
                            $http.post(baseUri + 'DesignOrder/Show_SendReview',
                            {
                                designId: $scope.design.Id,
                                files: $scope.design.Files
                            }).
                            success(function (data, status, headers, config) {
                                $scope.fetchLoading = false;
                                
                                var id = [];

                                if ($scope.order.IsPaidPrepayment == false)
                                    id.push("p" + $scope.order.Id);

                                if ($scope.order.IsPaidFinal == false)
                                    id.push("f" + $scope.order.Id);

                                $state.go('^.final-payment-preview', { id: id });

                                $modalInstance.close($scope.design.Files);
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
                }

                else if ($scope.acceptedFile != null &&
                ($scope.order.IsPaidPrepayment == true && $scope.order.IsPaidFinal == true)) {

                    ngDialog.open({
                        template: '<div style="font-family:yekan;font-size:14px;">' +
                            '<p>' +
                            'تأیید طرح روبرو از جانب شما به منزله اتمام پروسه طراحی برای این سفارش می باشد ' +
                            '</p>' +
                            '<div style="text-align:left;">' +
                            '<button class="btn btn-warning" ng-click="close()" type="button">انصراف</button>' +
                            '<button class="btn btn-primary" ng-click="confirm()" type="button" style="margin-right:4px;">تأیید</button>' +
                            '</div></div>',
                        plain: true,
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
                            $http.post(baseUri + 'DesignOrder/Show_SendReview',
                            {
                                designId: $scope.design.Id,
                                files: $scope.design.Files
                            }).
                            success(function (data, status, headers, config) {
                                $scope.fetchLoading = false;
                                $modalInstance.close($scope.design.Files);
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
                }

                else {
                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'DesignOrder/Show_SendReview',
                    {
                        designId: $scope.design.Id,
                        files: $scope.design.Files
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        $modalInstance.close($scope.design.Files);
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.fetchLoading = false;
                    });
                }
            };
        };
        /*=-=-=-=-=-=-=-=-= End Checkout =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Design =-=-=-=-=-=-=-=-=*/
        $scope.showDesignModal = function (designIndex) {
            var modalInstance = $modal.open({
                templateUrl: '/DesignContent.html',
                controller: ShowDesignCtrl,
                size: 'lg',
                resolve: {
                    design: function () {
                        var design = {
                            Id: $scope.designs[designIndex].Id,
                            Description: $scope.designs[designIndex].Description,
                            PersianRegisterDate: $scope.designs[designIndex].PersianRegisterDate,
                            Time: $scope.designs[designIndex].Time,
                            IsPaidPrepayment: $scope.designs[designIndex].IsPaidPrepayment,
                            IsPaidFinal: $scope.designs[designIndex].IsPaidFinal,
                            Price: $scope.designs[designIndex].Price,
                            Prepayment: $scope.designs[designIndex].Prepayment,
                            Files: []
                        };
                        for (i = 0; i < $scope.designs[designIndex].Files.length; i++) {
                            design.Files.push(_.clone($scope.designs[designIndex].Files[i]));
                        }
                        return design;
                    },
                    order: function () {
                        return {
                            Id: $scope.order.Id,
                            Price: $scope.order.Price,
                            Prepayment: $scope.order.Prepayment,
                            IsPaidPrepayment: $scope.order.IsPaidPrepayment,
                            IsPaidFinal: $scope.order.IsPaidFinal
                        };
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowDesignCtrl = function ($scope, $http, $modalInstance, design, order) {

            $scope.design = design;
            $scope.order = order;
            $scope.states = {
                None: 1,
                Accept: 2,
                Maybe: 3,
                UnAccept: 4
            };
            $scope.selectedFile = $scope.design.Files[0];
            $scope.selectedIndex = 0;
            $scope.acceptedFile = null;
            $scope.acceptedIndex = -1;

            $scope.selectFile = function (index) {
                $scope.selectedFile = $scope.design.Files[index];
                $scope.selectedIndex = index;
            };

            $scope.selectNextFile = function () {
                if ($scope.selectedIndex < $scope.design.Files.length - 1) {
                    $scope.selectedIndex++;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = 0;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.selectPrevFile = function () {
                if ($scope.selectedIndex > 0) {
                    $scope.selectedIndex--;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = $scope.design.Files.length - 1;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        };
        /*=-=-=-=-=-=-=-=-= End Design =-=-=-=-=-=-=-=-=*/

        //init
        $scope.fetchData();
        $scope.fetchDesigns();
    }]);