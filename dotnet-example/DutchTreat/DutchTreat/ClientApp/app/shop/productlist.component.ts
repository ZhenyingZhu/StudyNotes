import { Component } from '@angular/core';
import { DataService } from '../shared/dataService';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})
export class ProductList {
    constructor(private data: DataService) {
        this.products = data.products;
    }

    public products = [];
}