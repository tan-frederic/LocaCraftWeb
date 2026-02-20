import { Component, Input, ViewChild, ViewContainerRef, ComponentRef, OnChanges, SimpleChanges, Type, Output, EventEmitter, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-lateral-drawer',
  standalone: true,
  imports: [],
  templateUrl: './lateral-drawer.component.html',
  styleUrl: './lateral-drawer.component.css'
})
export class LateralDrawerComponent implements OnChanges, AfterViewInit {
  // Drawer state
  @Input() drawerTitle: string = "Drawer Title";
  @Input() 
  set componentToDisplay(value: Type<any> | null) {
    this._componentToDisplay = value;
    this.loadComponentIfReady();
  }
  get componentToDisplay(): Type<any> | null {
    return this._componentToDisplay;
  }
  private _componentToDisplay: Type<any> | null = null;

  @Input() componentData: any = null;

  @Output() drawerClosed = new EventEmitter<void>();

  showDrawer = false;

  @ViewChild('container', { read: ViewContainerRef }) container!: ViewContainerRef;

  private componentRef: ComponentRef<any> | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['componentData'] && this.componentRef) {
      this.updateComponentData();
    }
  }

  ngAfterViewInit(): void {
    this.loadComponentIfReady();
  }

  private loadComponentIfReady(): void {
    if (this.componentToDisplay && this.container) {
      this.loadComponent();
    }
  }

  private loadComponent(): void {
    if (this.componentRef) {
      this.componentRef.destroy();
      this.componentRef = null;
    }
    if (this.componentToDisplay) {
      this.componentRef = this.container.createComponent(this.componentToDisplay);
      this.updateComponentData();
    }
  }

  private updateComponentData(): void {
    if (this.componentRef && this.componentData) {
      Object.assign(this.componentRef.instance, this.componentData);
    }
  }

  closeDrawer(): void {
    this.showDrawer = false;
    this.drawerClosed.emit();
  }

  openDrawer(): void {
    this.showDrawer = true;
  }
}