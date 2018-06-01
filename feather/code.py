import analogio, digitalio, busio, pulseio
import board
import time

def get_half_voltage():
    return analogio.AnalogIn(board.D9)

def get_actuators(half_voltage):
    led = digitalio.DigitalInOut(board.D13)
    led.direction = digitalio.Direction.OUTPUT

    # Common actuator setting
    duty = int(1.5 / (half_voltage / 65535 * 2 * 3.3) * 65535)
    pwm = pulseio.PWMOut(board.D10, frequency=10000, duty_cycle=duty)

    # Catapillars
    a01 = digitalio.DigitalInOut(board.A2)
    a01.direction = digitalio.Direction.OUTPUT
    a02 = digitalio.DigitalInOut(board.A3)
    a02.direction = digitalio.Direction.OUTPUT
    b01 = digitalio.DigitalInOut(board.A4)
    b01.direction = digitalio.Direction.OUTPUT
    b02 = digitalio.DigitalInOut(board.A5)
    b02.direction = digitalio.Direction.OUTPUT

    # Dozer
    a11 = digitalio.DigitalInOut(board.D5)
    a11.direction = digitalio.Direction.OUTPUT
    a12 = digitalio.DigitalInOut(board.D6)
    a12.direction = digitalio.Direction.OUTPUT

    return [(a01, a02), (b01, b02), (a11, a12)]

def get_uart():
    return busio.UART(board.TX, board.RX, baudrate=115200)

def forward(actuators):
    actuators[0][0].value = 0
    actuators[0][1].value = 1
    actuators[1][0].value = 1
    actuators[1][1].value = 0

def back(actuators):
    actuators[0][0].value = 1
    actuators[0][1].value = 0
    actuators[1][0].value = 0
    actuators[1][1].value = 1

def right(actuators):
    actuators[0][0].value = 1
    actuators[0][1].value = 0
    actuators[1][0].value = 1
    actuators[1][1].value = 0

def left(actuators):
    actuators[0][0].value = 0
    actuators[0][1].value = 1
    actuators[1][0].value = 0
    actuators[1][1].value = 1

def stop(actuators):
    actuators[0][0].value = 0
    actuators[0][1].value = 0
    actuators[1][0].value = 0
    actuators[1][1].value = 0

def up(actuators):
    actuators[2][0].value = 1
    actuators[2][1].value = 0

def down(actuators):
    actuators[2][0].value = 0
    actuators[2][1].value = 1

def keep(actuators):
    actuators[2][0].value = 0
    actuators[2][1].value = 0

def main():
    half_voltage = get_half_voltage()
    actuators = get_actuators(half_voltage.value)
    uart = get_uart()

    while True:
        data = uart.read(1)
        if data and len(data) == 1:
            if data[0] == 0x00:
                forward(actuators)
            elif data[0] == 0x01:
                back(actuators)
            elif data[0] == 0x02:
                right(actuators)
            elif data[0] == 0x03:
                left(actuators)
            elif data[0] == 0x0f:
                stop(actuators)
            elif data[0] == 0x10:
                up(actuators)
            elif data[0] == 0x11:
                down(actuators)
            elif data[0] == 0x1f:
                keep(actuators)

        time.sleep(0.5)

if __name__ == "__main__":
    main()
