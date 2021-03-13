import { Component } from "@angular/core"
import { Order } from "../data/order";
import { ShopService } from "../data/shopService";

@Component({
    selector: "completed-order",
    templateUrl: "order.component.html",
    styleUrls: []
})

export class CompletedOrder {

    public order: Order = new Order();

    constructor(public data: ShopService) {
        this.order = data.order;
        data.clearCart();
    }
}