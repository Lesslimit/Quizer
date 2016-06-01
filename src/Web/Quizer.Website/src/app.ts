import {Router, RouterConfiguration} from 'aurelia-router'
import {useView} from 'aurelia-framework'

@useView('views/layout/app.html')
export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Quizer';

        config.map([
            { route: ['', 'tests'], name: 'tests', moduleId: 'viewmodels/tests', nav: true, title: 'Tests' },
            { route: 'students', name: 'students', moduleId: 'viewmodels/users', nav: true, title: 'Students' }
        ]);

        this.router = router;
    }
}
