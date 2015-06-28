App.controller('ReceiveMessageListController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.messages = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;
        $scope.resultCount = 0;

        $scope.fetchMessages = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'ReceiveMessage/Get', {
                params: {
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.messages = data.Data.List;
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

        $scope.generatePagation = function () {
            $scope.pages = [];
            if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
            if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
            $scope.pages.push($scope.pageIndex);
            if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
            if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
        };

        $scope.changePage = function (index) {
            $scope.fetchMessages($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchMessages($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchMessages($scope.pageIndex - 1);
            }
        };

        $scope.showMessage = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/MessageModalContent.html',
                controller: MessageCtrl,
                size: 'lg',
                resolve: {
                    message: function () {
                        return _.clone($scope.messages[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.messages[index].IsRead = true;
            }, function () {
                $scope.messages[index].IsRead = true;
            });
        };

        var MessageCtrl = function ($scope, $http, $modalInstance, message) {

            $scope.message = message;

            $scope.markAsRead = function () {
                $http.post(baseUri + 'ReceiveMessage/MarkAsRead',
                {
                    id: $scope.message.Id,
                }).
                success(function (data, status, headers, config) {
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                });
            };
            if ($scope.message.IsRead == false) {
                $scope.markAsRead();
            }

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        };

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
                    $http.post(baseUri + 'ReceiveMessage/Remove',
                    {
                        id: $scope.messages[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchMessages($scope.pageIndex);
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

        //init
        $scope.fetchMessages(1);
    }]);