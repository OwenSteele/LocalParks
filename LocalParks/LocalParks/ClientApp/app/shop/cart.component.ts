import { Component } from "@angular/core";
import { ShopService } from "../data/shopService";

@Component({
    selector: "user-cart",
    templateUrl: "cart.component.html",
    styleUrls: []
})
export class Cart {
    constructor(public data: ShopService) { }
}