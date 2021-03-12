import * as _ from "lodash";
import { OrderItem } from "./orderItem"

export class Order {
    orderId: number;
    dateCreated: Date;
    orderNumber: string;
    items: Array<OrderItem> = new Array<OrderItem>();

    get total(): number {
        return _.sum(_.map(this.items,
            i => i.unitPrice * i.quantity
        ));
    };
}