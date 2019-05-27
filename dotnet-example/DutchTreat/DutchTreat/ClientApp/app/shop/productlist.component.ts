import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/dataService';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})
export class ProductList implements OnInit {
    constructor(private data: DataService) {
        this.products = data.products;
    }

    public products = [];

    ngOnInit(): void {
        this.data.loadProducts().subscribe(success => {
            if (success)
                this.products = this.data.products;
        });
    }
}