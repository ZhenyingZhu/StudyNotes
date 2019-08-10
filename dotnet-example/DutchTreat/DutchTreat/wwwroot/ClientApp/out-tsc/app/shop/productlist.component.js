import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { DataService } from '../shared/dataService';
var ProductList = /** @class */ (function () {
    function ProductList(data) {
        this.data = data;
        this.products = [];
        this.products = data.products;
    }
    ProductList.prototype.ngOnInit = function () {
        var _this = this;
        this.data.loadProducts().subscribe(function (success) {
            if (success)
                _this.products = _this.data.products;
        });
    };
    ProductList.prototype.addProduct = function (product) {
        this.data.addToOrder(product);
    };
    ProductList = tslib_1.__decorate([
        Component({
            selector: "product-list",
            templateUrl: "productList.component.html",
            styleUrls: ["productList.component.css"]
        }),
        tslib_1.__metadata("design:paramtypes", [DataService])
    ], ProductList);
    return ProductList;
}());
export { ProductList };
//# sourceMappingURL=productlist.component.js.map