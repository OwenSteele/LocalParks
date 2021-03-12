import * as _ from "lodash";
export class Order {
    constructor() {
        this.items = new Array();
    }
    get total() {
        return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
    }
    ;
}
//# sourceMappingURL=order.js.map