import { InputAction } from '../loops/Input';
import { MessageProtocol, MessageTypes } from '../network/protocol';
import { RenderData } from '../render/debug-2d';

export class Jazz {
  private renderDataCallBack: (rd: RenderData) => void;
  private readonly renderDataDto = new RenderData();
  private localFrame = 0;

  constructor(renderDataCallBack: (rd: RenderData) => void) {
    this.renderDataCallBack = renderDataCallBack;
  }

  postMessage(message: MessageProtocol) {
    switch (message.type) {
      case MessageTypes.LOCAL_INPUT:
        this.tick(this.extractInput(message));
    }
  }

  private tick(ia: InputAction) {
    let frameTimeStart = performance.now();
    console.log('Input action');
    console.log(ia);

    let frameTimeDelta = performance.now() - frameTimeStart;

    this.renderDataCallBackExec(frameTimeDelta);
  }

  private extractInput(message: MessageProtocol): InputAction {
    this.localFrame = message.frame;
    return message.inputAction;
  }

  private renderDataCallBackExec(frameTime: number = 0) {
    this.renderDataDto.frameTime = frameTime;
    this.renderDataDto.frame = this.localFrame;
    this.renderDataCallBack(this.renderDataDto);
  }
}
