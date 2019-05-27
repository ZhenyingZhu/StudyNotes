import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from "rxjs/operators";
var DataService = /** @class */ (function () {
    // Replace those example data with calling API.
    //public products = [
    //    {
    //        title: "First product",
    //        price: 19.99
    //    },
    //    {
    //        title: "Second product",
    //        price: 9.99
    //    },
    //    {
    //        title: "Third product",
    //        price: 14.99
    //    },
    //];
    function DataService(http) {
        this.http = http;
        this.products = [];
    }
    DataService.prototype.loadProducts = function () {
        var _this = this;
        return this.http.get("api/products")
            .pipe(map(function (data) {
            _this.products = data;
            return true;
        }));
    };
    DataService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], DataService);
    return DataService;
}());
export { DataService };
//# sourceMappingURL=dataService.js.map