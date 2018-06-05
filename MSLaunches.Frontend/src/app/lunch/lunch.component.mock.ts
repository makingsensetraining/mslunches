import { Component, Input, NgModule } from '@angular/core';

@Component({
    selector: 'app-lunch',
    template: '<p>mockedLunches</p>'
})
export class MockedLunchComponent {
    @Input() canBeSelected: Boolean;
}

@NgModule({
    declarations: [MockedLunchComponent],
    exports: [MockedLunchComponent]
})
export class MockedLunchModule {}
