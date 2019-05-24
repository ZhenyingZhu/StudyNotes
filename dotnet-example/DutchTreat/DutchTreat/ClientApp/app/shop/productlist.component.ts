import { Component } from '@angular/core';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})
export class ProductList {
    public products = [
        {
            title: "First product",
            price: 19.99
        },
        {
            title: "Second product",
            price: 9.99
        },
        {
            title: "Third product",
            price: 14.99
        },
    ];
}