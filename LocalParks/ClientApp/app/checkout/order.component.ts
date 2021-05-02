import { Component } from "@angular/core"
import { Order } from "../data/order";
import { ShopService } from "../data/shopService";

@Component({
    selector: "completed-order",
    templateUrl: "order.component.html",
    styleUrls: ["checkout.component.css"]
})

export class CompletedOrder {

    public order: Order = new Order();

    constructor(public data: ShopService) {
        this.order = data.order;
        console.log(JSON.stringify(this.order));
        data.clearCart();
        console.log(JSON.stringify(this.order));
    }
}