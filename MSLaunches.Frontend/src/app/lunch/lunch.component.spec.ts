import { ComponentFixture, TestBed, fakeAsync, tick, async } from '@angular/core/testing';
import { LunchComponent } from '@app/lunch/lunch.component';
import { LunchTileComponent } from '@app/lunch/lunch-tile/lunch-tile.component';
import { MockLunchService } from '@app/lunch/lunch.service.mock';
import { LunchService } from '@app/lunch/lunch.service';
import { SharedModule } from '@app/shared';
import { of } from 'rxjs/observable/of';

describe('LunchComponent', () => {
    let fixture: ComponentFixture<LunchComponent>;
    let component: LunchComponent;
    let lunchService: LunchService;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                SharedModule
            ],
            declarations: [
                LunchComponent,
                LunchTileComponent
            ],
            providers: [
                {
                    useClass: MockLunchService,
                    provide: LunchService
                }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(LunchComponent);
        lunchService = TestBed.get(LunchService);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should get lunches on init', () => {
        expect(component.lunches).toBeTruthy();
        expect(component.lunches.length).toBeGreaterThan(0);
    });

    it('set lunch selected, with a selected lunch, should put every else in false', () => {
        const lunch = component.lunches[0].lunches[0].lunches[0];
        lunch.isSelected = true;
        component.setLunchSelected(lunch);
        component.lunches[0].lunches[0].lunches.forEach(s => {
            if (lunch.userLunchId !== s.userLunchId) {
                expect(s.isSelected).toBeFalsy();
            }
        });
    });

    it('set lunch selected, should save it', fakeAsync(() => {
        const lunch = component.lunches[0].lunches[0].lunches[0];
        lunch.isSelected = true;
        spyOn(lunchService, 'save').and.returnValue(of('someString'));
        component.setLunchSelected(lunch);
        tick();
        expect(lunchService.save).toHaveBeenCalledTimes(1);
    }));

    it('set lunch unselected, should remove it', () => {
        const lunch = component.lunches[0].lunches[0].lunches[0];
        lunch.isSelected = false;
        spyOn(lunchService, 'delete').and.returnValue(of('someString'));
        component.setLunchSelected(lunch);

        expect(lunchService.delete).toHaveBeenCalledTimes(1);
    });
});
