import { ComponentFixture, TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { CoreModule } from '@app/core';
import { UserLunch } from '@app/core/Models/user-lunch.model';
import { LunchTileComponent } from '@app/lunch/lunch-tile/lunch-tile.component';

const mockedLunch: UserLunch = {
    date: new Date(Date.now()),
    id: 'id',
    isSelectable: true,
    isSelected: false,
    type: 'sometype',
    userLunchId: 'userid',
    description: 'desc'
};

describe('LunchTileComponent', () => {
    let component: LunchTileComponent;
    let fixture: ComponentFixture<LunchTileComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                NgbModule.forRoot(),
                RouterTestingModule,
                CoreModule
            ],
            declarations: [LunchTileComponent]
        });
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(LunchTileComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(!!component).toBeTruthy();
    });

    it('select should change lunch selection', () => {
        component.lunch = mockedLunch;

        component.select();

        expect(component.lunch.isSelected).toBeTruthy();
    });

    it('select should emit the lunch', () => {
        component.lunch = mockedLunch;
        spyOn(component.lunchSelected, 'emit');

        component.select();

        fixture.detectChanges();
        expect(component.lunchSelected.emit).toHaveBeenCalledTimes(1);
    });
});
