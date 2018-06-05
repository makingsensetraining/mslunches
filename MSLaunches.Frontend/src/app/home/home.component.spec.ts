import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { HomeComponent } from './home.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MockedLunchModule } from '@app/lunch/lunch.component.mock';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        imports: [
          CoreModule,
          SharedModule,
          HttpClientTestingModule,
          RouterTestingModule,
          MockedLunchModule
        ],
        declarations: [
          HomeComponent,
        ],
        providers: []
      })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    component.isLoading = false;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have loader disabled', () => {
    expect(component.isLoading).toBeFalsy();
  });
});
