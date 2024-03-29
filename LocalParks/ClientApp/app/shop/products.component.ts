﻿import { Component, OnInit } from "@angular/core";
import { ShopService } from "../data/shopService";
import { Product } from "../data/product";

@Component({
    selector: "shop-products",
    templateUrl: "products.component.html",
    styleUrls: ["products.component.css"]
})
export class Products implements OnInit {

    public products: Product[];
    public memberships: Product[];
    public loading: boolean = true;

    constructor(private data: ShopService) {
    }

    ngOnInit() {
        this.data.getProducts()
            .subscribe(success => {
                if (success) {
                    this.products = this.data.products;
                    this.loading = false;
                }
            });
    }

    addProduct(product: Product) {
        this.data.AddItemToOrder(product);
    }
}