import threading
import time

# Creamos dos cerrojos (locks)
lock1 = threading.Lock()
lock2 = threading.Lock()

# Función que simula el trabajo del hilo 1
def thread1():
    print("Hilo 1: Intentando adquirir Lock 1...")
    lock1.acquire()
    print("Hilo 1: Lock 1 adquirido.")
    time.sleep(1)  # Simulamos que está realizando alguna tarea
    
    print("Hilo 1: Intentando adquirir Lock 2...")
    lock2.acquire()
    print("Hilo 1: Lock 2 adquirido.")
    
    # Liberamos ambos locks
    lock2.release()
    lock1.release()

# Función que simula el trabajo del hilo 2
def thread2():
    print("Hilo 2: Intentando adquirir Lock 2...")
    lock2.acquire()
    print("Hilo 2: Lock 2 adquirido.")
    time.sleep(1)  # Simulamos que está realizando alguna tarea
    
    print("Hilo 2: Intentando adquirir Lock 1...")
    lock1.acquire()
    print("Hilo 2: Lock 1 adquirido.")
    
    # Liberamos ambos locks
    lock1.release()
    lock2.release()

# Crear hilos
t1 = threading.Thread(target=thread1)
t2 = threading.Thread(target=thread2)

# Iniciar hilos
t1.start()
t2.start()

# Esperar a que ambos hilos terminen
t1.join()
t2.join()

print("Programa terminado.")
