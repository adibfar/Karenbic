/**=========================================================
 * Module: form-list.js
 * Create Forms List
 =========================================================*/

App.controller('FormsListController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $state) {
        $scope.forms = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.fetchForms = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'Form/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2,
                    title: $scope.title,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.forms = data.Data.List;
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
            $scope.fetchForms(1);
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
            $scope.fetchForms($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchForms($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchForms($scope.pageIndex - 1);
            }
        };

        $scope.edit = function (index) {
            $scope.fetchLoading = true;
            $state.go('^.edit-form', { id: $scope.forms[index].Id });
        };

        $scope.show = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'Form/Show',
            {
                id: $scope.forms[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True") {
                    $scope.forms[index].IsShow = true;
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
        };

        $scope.hide = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'Form/Hide',
            {
                id: $scope.forms[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True") {
                    $scope.forms[index].IsShow = false;
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
                    if (value == 0) return true;

                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'Form/Remove',
                    {
                        id: $scope.forms[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchForms($scope.pageIndex);
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

        //Init
        $scope.fetchForms(1);
    }]);