import {Router, RouterConfiguration} from 'aurelia-router'
import {useView} from 'aurelia-framework'

@useView('views/layout/app.html')
export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Quizer';

        config.map([
            { route: 'profile', name: 'profile', moduleId: 'viewmodels/profile', nav: true, title: 'Профіль' },
            { route: ['', 'tests'], name: 'tests', moduleId: 'viewmodels/tests', nav: true, title: 'Тести' },
            { route: 'test/:id', moduleId: 'viewmodels/test', nav: false, href: 'test/:id' },
            { route: 'stats', name: 'stats', moduleId: 'viewmodels/stats', nav: true, title: 'Статистика' },
            { route: 'group', name: 'group', moduleId: 'viewmodels/group', nav: true, title: 'Група' }
        ]);

        this.router = router;
    }
}
