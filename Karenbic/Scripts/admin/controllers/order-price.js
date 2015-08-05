App.controller('OrderPriceController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.formGroup = {};
        $scope.formGroups = [];
        $scope.form = {};
        $scope.forms = [];

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

    }]);