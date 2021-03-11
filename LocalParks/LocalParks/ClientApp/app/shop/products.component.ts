import { Component, OnInit } from "@angular/core";
import { ShopService } from "../shared/shopService";
import { Product } from "../shared/product";

@Component({
    selector: "shop-products",
    templateUrl: "Products.component.html",
    styleUrls: []
})
export class Products implements OnInit {
    public products: Product[];

    constructor(private data: ShopService) {
    }

    ngOnInit() {
        this.data.getProducts()
            .subscribe(success => {
                if (success) {
                    this.products = this.data.products;
                }
            });
    }

    addProduct(product: Product) {
        this.data.AddItemToOrder(product);
    }
}