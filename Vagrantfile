Vagrant.configure("2") do |config|
    config.vm.box = "win10_ltsc_2019"
    config.vm.guest = :windows
    config.vm.communicator = "winrm"
    # 3389 RDP
    config.vm.network "forwarded_port", guest: 3389, host: 3389
end