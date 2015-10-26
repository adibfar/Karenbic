/**=========================================================
 * Module: print-order-list.js
 * Show Order List
 =========================================================*/

App.controller('PrintOrderListController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {
        $scope.searchFields = {
            orderId: '',
            startDate: '',
            endDate: ''
        };
        $scope.orders = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.fetchOrders = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'PrintOrder/Get', {
                params: {
                    orderId: $scope.searchFields.orderId.trim() != '' ? Number($scope.searchFields.orderId) : null,
                    startDate: $scope.searchFields.startDate,
                    endDate: $scope.searchFields.endDate,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.orders = data.Data.List;
                $scope.pageCount = data.Data.PageCount;
                $scope.pageIndex = data.Data.PageIndex;
                $scope.resultCount = data.Data.ResultCount;
                $scope.generatePagation();
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

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.fetchOrders(1);
        }

        $scope.generatePagation = function () {
            $scope.pages = [];
            if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
            if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
            $scope.pages.push($scope.pageIndex);
            if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
            if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
        };

        $scope.changePage = function (index) {
            $scope.fetchOrders($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchOrders($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchOrders($scope.pageIndex - 1);
            }
        };

        $scope.showDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowDetailsContent.html',
                controller: ShowDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.orders[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.orders[index].Price = result;
                $scope.orders[index].IsConfirm = true;
            }, function () {
            });
        };

        var ShowDetailsCtrl = ['$scope', '$http', '$modalInstance', 'order', function ($scope, $http, $modalInstance, order) {

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

            $scope.save = function () {
                $scope.confirmOrderFromSubmited = true;
                if ($scope.confirmOrderFrom.$invalid) return;

                $scope.confirmLoading = true;
                $http.post(baseUri + 'PrintOrder/Confirm',
                {
                    orderId: $scope.order.Id,
                    price: $scope.order.Price
                }).
                success(function (data, status, headers, config) {
                    $scope.confirmLoading = false;
                    if (data == "True") {
                        $modalInstance.close($scope.order.Price);
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
                    $scope.confirmLoading = false;
                });
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.showPaymentDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowPaymentDetailsContent.html',
                controller: ShowPaymentDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.orders[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowPaymentDetailsCtrl = ['$scope', '$http', '$modalInstance', 'order', function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'PrintPayment/Details', {
                    params: {
                        id: $scope.order.Payment.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.payment = data.Data.payment;
                    $scope.factors = data.Data.factors;
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

            $scope.totalPrice = function () {
                if ($scope.factors == null || $scope.factors == undefined || $scope.factors.length == 0) return 0;

                var sum = _.reduce($scope.factors, function (memo, item) { return memo + item.Price; }, 0);

                return sum;
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        //order files
        $scope.fileType = {
            Unknown: 0,
            Image: 1,
            Word: 2,
            PDF: 3,
            TXT: 4,
            ZIP: 5,
            RAR: 6,
            TIFF: 7
        };
        $scope.checkFileType = function (filename) {
            var extension = filename.substr(filename.lastIndexOf('.') + 1).toLowerCase();
            switch (extension) {
                case 'jpg':
                case 'jpeg':
                case 'png':
                case 'gif':
                    return $scope.fileType.Image;
                    break;
                case 'doc':
                case 'docx':
                    return $scope.fileType.Word;
                    break;
                case 'pdf':
                    return $scope.fileType.PDF;
                    break;
                case 'txt':
                    return $scope.fileType.TXT;
                    break;
                case 'zip':
                    return $scope.fileType.ZIP;
                    break;
                case 'rar':
                    return $scope.fileType.RAR;
                    break;
                case 'tiff':
                    return $scope.fileType.TIFF;
                    break;
                default:
                    return $scope.fileType.Unknown;
            }
        };

        //Init
        $scope.fetchOrders(1);
    }]);