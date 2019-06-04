import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import * as OrderNS from './order';
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
        this.order = new OrderNS.Order();
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
    DataService.prototype.addToOrder = function (newProduct) {
        var item = this.order.items.find(function (i) { return i.productId == newProduct.id; });
        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderNS.OrderItem();
            item.productId = newProduct.id;
            item.productArtist = newProduct.artist;
            item.productArtId = newProduct.artId;
            item.productCategory = newProduct.category;
            item.productSize = newProduct.size;
            item.productTitle = newProduct.title;
            item.unitPrice = newProduct.price;
            item.quantity = 1;
            this.order.items.push(item);
        }
    };
    DataService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], DataService);
    return DataService;
}());
export { DataService };
//# sourceMappingURL=dataService.js.map