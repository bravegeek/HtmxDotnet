export class DebugRenderer {
  private ctx: CanvasRenderingContext2D;
  private xRes: number;
  private yRes: number;

  constructor(canvas: HTMLCanvasElement, res: resolution) {
    this.ctx = canvas.getContext('2d')!;
    this.xRes = res.x;
    this.yRes = res.y;
  }

  render(renderDataDTO: RenderData) {
    const ctx = this.ctx;
    ctx.clearRect(0, 0, this.xRes, this.yRes);
    ctx.fillStyle = 'white';
    ctx.fillText(`Frame: ${renderDataDTO.frame}`, 10, 30);
    ctx.fillText(`FrameTime: ${renderDataDTO.frameTime}`, 10, 60);
  }
}

export type resolution = {
  x: number;
  y: number;
};

export class RenderData {
  frame: number = 0;
  frameTime: number = 0;

  constructor() {}
}
