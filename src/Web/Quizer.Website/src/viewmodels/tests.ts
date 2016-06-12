import {useView} from 'aurelia-framework';

@useView('views/tests.html')
export class Tests {

    get heading() {
        return 'Тести';
    }
}