import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from "rxjs/operators";

@Injectable()
export class DataService {
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

    constructor(private http: HttpClient) {
    }

    public products = [];

    loadProducts() {
        return this.http.get("api/products")
            .pipe(map((data: any[]) => {
                this.products = data;
                return true;
            }));
    }
}