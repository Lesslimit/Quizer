import {useView} from 'aurelia-framework';
import {dataService} from "../dataService/dataService"

@useView('views/tests.html')
export class Tests {
    tests: any[] = []
    get heading() {
        return '�����';
    }

    activate() {
        dataService.tests.getAll().then(data => {
            this.tests.push(data);
        });
    }
}